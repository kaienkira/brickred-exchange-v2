using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Brickred.Exchange.Compiler
{
    public sealed class ProtocolParser : IDisposable
    {
        private ProtocolDescriptor descriptor = null;
        public ProtocolDescriptor Descriptor
        {
            get { return this.descriptor; }
        }

        public ProtocolParser()
        {
        }

        ~ProtocolParser()
        {
            Dispose();
        }

        public bool Parse(
            string protoFilePath, List<string> protoSearchPath)
        {
            this.descriptor = new ProtocolDescriptor();
            this.descriptor.ProtoDef =
                ParseProtocol(protoFilePath, protoSearchPath);

            if (this.descriptor.ProtoDef == null) {
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            if (this.descriptor != null) {
                this.descriptor = null;
            }
        }

        private int GetLineNumber(XElement element)
        {
            IXmlLineInfo lineInfo = (IXmlLineInfo)element;
            return lineInfo.LineNumber;
        }

        private void PrintLineError(
            string fileName, int lineNumber,
            string format, params object[] args)
        {
            Console.Error.WriteLine(
                string.Format("error:{0}:{1}: ", fileName, lineNumber) +
                string.Format(format, args));
        }

        private void PrintLineError(
            ProtocolDescriptor.ProtocolDef protoDef, XElement element,
            string format, params object[] args)
        {
            PrintLineError(protoDef.FilePath, GetLineNumber(element),
                format, args);
        }

        private string GetProtoFileFullPath(
            string protoFilePath, List<string> protoSearchPath)
        {
            bool fileExists = false;
            do {
                // find proto file path directly first
                if (File.Exists(protoFilePath)) {
                    fileExists = true;
                    break;
                }

                // find in the search path
                for (int i = 0; i < protoSearchPath.Count; ++i) {
                    string checkPath = Path.Combine(
                        protoSearchPath[i], protoFilePath);
                    if (File.Exists(checkPath)) {
                        fileExists = true;
                        protoFilePath = checkPath;
                        break;
                    }
                }
            } while (false);

            if (fileExists) {
                return Path.GetFullPath(protoFilePath);
            } else {
                return "";
            }
        }

        private XDocument LoadProtoFile(string filePath)
        {
            XDocument xmlDoc = null;
            try {
                xmlDoc = XDocument.Load(
                    filePath, LoadOptions.SetLineInfo);
            } catch (Exception e) {
                Console.Error.WriteLine(string.Format(
                    "error: can not load protocol file `{0}`: {1}",
                    filePath, e.Message));
                return null;
            }

            return xmlDoc;
        }

        private ProtocolDescriptor.ProtocolDef ParseProtocol(
            string protoFilePath, List<string> protoSearchPath)
        {
            ProtocolDescriptor.ProtocolDef protoDef = null;

            // check is already imported
            string protoName = Path.GetFileNameWithoutExtension(protoFilePath);
            if (this.descriptor.ImportedProtos.TryGetValue(
                    protoName, out protoDef)) {
                return protoDef;
            }

            // get file full path
            string protoFileFullPath = GetProtoFileFullPath(
                protoFilePath, protoSearchPath);
            if (protoFileFullPath == "") {
                Console.Error.WriteLine(string.Format(
                    "error: can not find protocol file `{0}`", protoFilePath));
                return null;
            }

            // load xml doc
            XDocument xmlDoc = LoadProtoFile(protoFileFullPath);
            if (xmlDoc == null) {
                return null;
            }

            protoDef = new ProtocolDescriptor.ProtocolDef();
            protoDef.Name = protoName;
            protoDef.FilePath = protoFileFullPath;

            // add to imported cache first to prevent circular import
            this.descriptor.ImportedProtos.Add(protoName, protoDef);

            // check root node name
            if (xmlDoc.Root.Name != "protocol") {
                PrintLineError(protoDef, xmlDoc.Root,
                    "root node must be `protocol` node");
                return null;
            }

            // parse imports
            {
                IEnumerator<XElement> iter = xmlDoc.XPathSelectElements(
                    "/protocol/import").GetEnumerator();
                while (iter.MoveNext()) {
                    XElement element = iter.Current;

                    // check import self
                    string refProtoName =
                        Path.GetFileNameWithoutExtension(element.Value);
                    if (refProtoName == protoDef.Name) {
                        PrintLineError(protoDef, element,
                            "can not import self");
                        return null;
                    }

                    ProtocolDescriptor.ProtocolDef externalProtoDef =
                        ParseProtocol(element.Value, protoSearchPath);
                    if (externalProtoDef == null) {
                        PrintLineError(protoDef, element,
                            "load external file `{0}` failed",
                            element.Value);
                        return null;
                    }

                    if (AddImportDef(protoDef, element,
                            externalProtoDef) == false) {
                        return null;
                    }
                }
            }

            // parse namespaces
            {
                IEnumerator<XElement> iter = xmlDoc.XPathSelectElements(
                    "/protocol/namespace").GetEnumerator();
                while (iter.MoveNext()) {
                    XElement element = iter.Current;

                    if (AddNamespaceDef(protoDef, element) == false) {
                        return null;
                    }
                }
            }

            // parse enums
            {
                IEnumerator<XElement> iter = xmlDoc.XPathSelectElements(
                    "/protocol/enum").GetEnumerator();
                while (iter.MoveNext()) {
                    XElement element = iter.Current;

                    if (AddEnumDef(protoDef, element) == false) {
                        return null;
                    }
                }
            }

            // parse structs
            {
                IEnumerator<XElement> iter = xmlDoc.XPathSelectElements(
                    "/protocol/struct").GetEnumerator();
                while (iter.MoveNext()) {
                    XElement element = iter.Current;

                    if (AddStructDef(protoDef, element) == false) {
                        return null;
                    }
                }
            }

            // parse enum maps
            {
                IEnumerator<XElement> iter = xmlDoc.XPathSelectElements(
                    "/protocol/enum_map").GetEnumerator();
                while (iter.MoveNext()) {
                    XElement element = iter.Current;

                    if (AddEnumMapDef(protoDef, element) == false) {
                        return null;
                    }
                }
            }

            ProcessImportedProtocols(protoDef);

            return protoDef;
        }

        private bool AddImportDef(
            ProtocolDescriptor.ProtocolDef protoDef, XElement element,
            ProtocolDescriptor.ProtocolDef externalProtoDef)
        {
            ProtocolDescriptor.ImportDef def =
                new ProtocolDescriptor.ImportDef();
            def.ParentRef = protoDef;
            def.Name = externalProtoDef.Name;
            def.LineNumber = GetLineNumber(element);
            def.ProtoDef = externalProtoDef;

            if (protoDef.ImportNameIndex.ContainsKey(def.Name)) {
                PrintLineError(protoDef, element,
                    "import `{0}` duplicated", def.Name);
                return false;
            }

            protoDef.Imports.Add(def);
            protoDef.ImportNameIndex.Add(def.Name, def);

            return true;
        }

        private bool AddNamespaceDef(
            ProtocolDescriptor.ProtocolDef protoDef, XElement element)
        {
            // check lang attr
            string lang;
            {
                XAttribute attr = element.Attribute("lang");
                if (attr == null) {
                    PrintLineError(protoDef, element,
                        "`namespace` node must contain a `lang` attribute");
                    return false;
                }
                lang = attr.Value;
            }
            if (protoDef.Namespaces.ContainsKey(lang)) {
                PrintLineError(protoDef, element,
                    "namespace node `lang` attribute duplicated");
                return false;
            }

            // check namespace value
            if (element.Value == "") {
                PrintLineError(protoDef, element,
                    "`namespace` node value can not be empty");
                return false;
            }

            // check namespace parts
            string[] namespaceParts = element.Value.Split('.');
            for (int i = 0; i < namespaceParts.Length; ++i) {
                if (Regex.IsMatch(namespaceParts[i],
                        @"^[a-zA-Z_]\w*$") == false) {
                    PrintLineError(protoDef, element,
                        "`namespace` node value is invalid");
                    return false;
                }
            }

            ProtocolDescriptor.NamespaceDef def =
                new ProtocolDescriptor.NamespaceDef();
            def.ParentRef = protoDef;
            def.Language = lang;
            def.LineNumber = GetLineNumber(element);
            def.Namespace = element.Value;
            for (int i = 0; i < namespaceParts.Length; ++i) {
                def.NamespaceParts.Add(namespaceParts[i]);
            }

            protoDef.Namespaces.Add(def.Language, def);

            return true;
        }

        private bool AddEnumDef(
            ProtocolDescriptor.ProtocolDef protoDef, XElement element)
        {
            // check name attr
            string name;
            {
                XAttribute attr = element.Attribute("name");
                if (attr == null) {
                    PrintLineError(protoDef, element,
                        "`enum` node must contain a `name` attribute");
                    return false;
                }
                name = attr.Value;
            }
            if (Regex.IsMatch(name, @"^[a-zA-Z_]\w*$") == false) {
                PrintLineError(protoDef, element,
                    "`enum` node `name` attribute is invalid");
                return false;
            }
            if (protoDef.EnumNameIndex.ContainsKey(name) ||
                protoDef.StructNameIndex.ContainsKey(name) ||
                protoDef.EnumMapNameIndex.ContainsKey(name)) {
                PrintLineError(protoDef, element,
                    "`enum` node `name` attribute duplicated");
                return false;
            }

            ProtocolDescriptor.EnumDef def =
                new ProtocolDescriptor.EnumDef();
            def.ParentRef = protoDef;
            def.Name = name;
            def.LineNumber = GetLineNumber(element);

            // parse items
            {
                IEnumerator<XElement> iter =
                    element.Elements().GetEnumerator();
                while (iter.MoveNext()) {
                    XElement childElement = iter.Current;

                    if (childElement.Name != "item") {
                        PrintLineError(protoDef, childElement,
                            "expect a `item` node");
                        return false;
                    }

                    if (AddEnumItemDef(protoDef, def,
                            childElement) == false) {
                        return false;
                    }
                }
            }

            protoDef.Enums.Add(def);
            protoDef.EnumNameIndex.Add(def.Name, def);

            return true;
        }

        private bool AddEnumItemDef(
            ProtocolDescriptor.ProtocolDef protoDef,
            ProtocolDescriptor.EnumDef enumDef, XElement element)
        {
            // check name attr
            string name;
            {
                XAttribute attr = element.Attribute("name");
                if (attr == null) {
                    PrintLineError(protoDef, element,
                        "`item` node must contain a `name` attribute");
                    return false;
                }
                name = attr.Value;
            }
            if (Regex.IsMatch(name, @"^[a-zA-Z_]\w*$") == false) {
                PrintLineError(protoDef, element,
                    "`item` node `name` attribute is invalid");
                return false;
            }
            if (enumDef.ItemNameIndex.ContainsKey(name)) {
                PrintLineError(protoDef, element,
                    "`item` node `name` attribute duplicated");
                return false;
            }

            // check value attr
            string val = "";
            {
                XAttribute attr = element.Attribute("value");
                if (attr != null) {
                    val = attr.Value;
                }
            }

            ProtocolDescriptor.EnumDef.ItemDef def =
                new ProtocolDescriptor.EnumDef.ItemDef();
            def.ParentRef = enumDef;
            def.Name = name;
            def.LineNumber = GetLineNumber(element);

            if (val == "") {
                // default
                def.Type = ProtocolDescriptor.EnumDef.ItemType.Default;
                if (enumDef.Items.Count == 0) {
                    def.IntValue = 0;
                } else {
                    def.IntValue =
                        enumDef.Items[enumDef.Items.Count - 1].IntValue + 1;
                }
            } else if (Regex.IsMatch(val, @"^[0-9]+$")) {
                // int
                def.Type = ProtocolDescriptor.EnumDef.ItemType.Int;
                def.IntValue = int.TryParse(val, out def.IntValue)
                    ? def.IntValue : 0;
            } else {
                string[] parts = val.Split('.');
                if (parts.Length == 1) {
                    // current enum
                    string refDefName = parts[0];

                    ProtocolDescriptor.EnumDef.ItemDef refDef = null;
                    if (enumDef.ItemNameIndex.TryGetValue(
                            refDefName, out refDef) == false) {
                        PrintLineError(protoDef, element,
                            "enum item `{0}` is undefined", refDefName);
                        return false;
                    }

                    def.Type =
                        ProtocolDescriptor.EnumDef.ItemType.CurrentEnumRef;
                    def.IntValue = refDef.IntValue;
                    def.RefEnumItemDef = refDef;

                } else if (parts.Length == 2) {
                    // other enum in same file
                    string refEnumDefName = parts[0];
                    string refDefName = parts[1];

                    ProtocolDescriptor.EnumDef refEnumDef = null;
                    if (protoDef.EnumNameIndex.TryGetValue(
                            refEnumDefName, out refEnumDef) == false) {
                        PrintLineError(protoDef, element,
                            "enum `{0}` is undefined", refEnumDefName);
                        return false;
                    }
                    ProtocolDescriptor.EnumDef.ItemDef refDef = null;
                    if  (refEnumDef.ItemNameIndex.TryGetValue(
                            refDefName, out refDef) == false) {
                        PrintLineError(protoDef, element,
                            "enum item `{0}` is undefined", refDefName);
                        return false;
                    }

                    def.Type =
                        ProtocolDescriptor.EnumDef.ItemType.OtherEnumRef;
                    def.IntValue = refDef.IntValue;
                    def.RefEnumItemDef = refDef;

                } else if (parts.Length == 3) {
                    // other enum in other file
                    string refProtoDefName = parts[0];
                    string refEnumDefName = parts[1];
                    string refDefName = parts[2];

                    ProtocolDescriptor.ProtocolDef refProtoDef = null;
                    if (this.descriptor.ImportedProtos.TryGetValue(
                            refProtoDefName, out refProtoDef) == false) {
                        PrintLineError(protoDef, element,
                            "protocol `{0}` is undefined", refProtoDefName);
                        return false;
                    }
                    ProtocolDescriptor.EnumDef refEnumDef = null;
                    if (refProtoDef.EnumNameIndex.TryGetValue(
                            refEnumDefName, out refEnumDef) == false) {
                        PrintLineError(protoDef, element,
                            "enum `{0}` is undefined", refEnumDefName);
                        return false;
                    }
                    ProtocolDescriptor.EnumDef.ItemDef refDef = null;
                    if  (refEnumDef.ItemNameIndex.TryGetValue(
                            refDefName, out refDef) == false) {
                        PrintLineError(protoDef, element,
                            "enum item `{0}` is undefined", refDefName);
                        return false;
                    }

                    def.Type =
                        ProtocolDescriptor.EnumDef.ItemType.OtherEnumRef;
                    def.IntValue = refDef.IntValue;
                    def.RefEnumItemDef = refDef;
                } else {
                    PrintLineError(protoDef, element,
                        "enum value `{0}` is invalid", val);
                    return false;
                }
            }

            enumDef.Items.Add(def);
            enumDef.ItemNameIndex.Add(def.Name, def);

            return true;
        }

        private bool AddStructDef(
            ProtocolDescriptor.ProtocolDef protoDef, XElement element)
        {
            // check name attr
            string name;
            {
                XAttribute attr = element.Attribute("name");
                if (attr == null) {
                    PrintLineError(protoDef, element,
                        "`struct` node must contain a `name` attribute");
                    return false;
                }
                name = attr.Value;
            }
            if (Regex.IsMatch(name, @"^[a-zA-Z_]\w*$") == false) {
                PrintLineError(protoDef, element,
                    "`struct` node `name` attribute is invalid");
                return false;
            }
            if (protoDef.StructNameIndex.ContainsKey(name) ||
                protoDef.EnumNameIndex.ContainsKey(name) ||
                protoDef.EnumMapNameIndex.ContainsKey(name)) {
                PrintLineError(protoDef, element,
                    "`struct` node `name` attribute duplicated");
                return false;
            }

            ProtocolDescriptor.StructDef def =
                new ProtocolDescriptor.StructDef();
            def.ParentRef = protoDef;
            def.Name = name;
            def.LineNumber = GetLineNumber(element);

            // parse fields
            {
                IEnumerator<XElement> iter =
                    element.Elements().GetEnumerator();
                while (iter.MoveNext()) {
                    XElement childElement = iter.Current;

                    if (childElement.Name != "required" &&
                        childElement.Name != "optional") {
                        PrintLineError(protoDef, childElement,
                            "expect a `required` or `optional` node");
                        return false;
                    }

                    if (AddStructFieldDef(protoDef, def,
                            childElement) == false) {
                        return false;
                    }
                }
            }

            if (def.OptionalFieldCount > 0) {
                def.OptionalByteCount =
                    def.OptionalFieldCount / 8 + 1;
            }

            protoDef.Structs.Add(def);
            protoDef.StructNameIndex.Add(def.Name, def);

            return true;
        }

        private bool AddStructFieldDef(
            ProtocolDescriptor.ProtocolDef protoDef,
            ProtocolDescriptor.StructDef structDef, XElement element)
        {
            // check name attr
            string name;
            {
                XAttribute attr = element.Attribute("name");
                if (attr == null) {
                    PrintLineError(protoDef, element,
                        "`{0}` node must contain a `name` attribute",
                        element.Name);
                    return false;
                }
                name = attr.Value;
            }
            if (Regex.IsMatch(name, @"^[a-zA-Z_]\w*$") == false) {
                PrintLineError(protoDef, element,
                    "`{0}` node `name` attribute is invalid",
                    element.Name);
                return false;
            }
            if (structDef.FieldNameIndex.ContainsKey(name)) {
                PrintLineError(protoDef, element,
                    "`{0}` node `name` attribute duplicated",
                    element.Name);
                return false;
            }

            // check type attr
            string type;
            {
                XAttribute attr = element.Attribute("type");
                if (attr == null) {
                    PrintLineError(protoDef, element,
                        "`{0}` node must contain a `type` attribute",
                        element.Name);
                    return false;
                }
                type = attr.Value;
            }

            ProtocolDescriptor.StructDef.FieldDef def =
                new ProtocolDescriptor.StructDef.FieldDef();
            def.ParentRef = structDef;
            def.Name = name;
            def.LineNumber = GetLineNumber(element);

            // get type info
            string fieldTypeStr = type;
            {
                Match m = Regex.Match(type, @"^list{(.+)}$");
                if (m.Success) {
                    fieldTypeStr = m.Groups[1].Value;
                    def.Type = ProtocolDescriptor.StructDef.FieldType.List;
                }
            }

            ProtocolDescriptor.StructDef.FieldType fieldType =
                ProtocolDescriptor.StructDef.FieldType.None;
            if (fieldTypeStr == "i8") {
                fieldType = ProtocolDescriptor.StructDef.FieldType.I8;
            } else if (fieldTypeStr == "u8") {
                fieldType = ProtocolDescriptor.StructDef.FieldType.U8;
            } else if (fieldTypeStr == "i16") {
                fieldType = ProtocolDescriptor.StructDef.FieldType.I16;
            } else if (fieldTypeStr == "u16") {
                fieldType = ProtocolDescriptor.StructDef.FieldType.U16;
            } else if (fieldTypeStr == "i32") {
                fieldType = ProtocolDescriptor.StructDef.FieldType.I32;
            } else if (fieldTypeStr == "u32") {
                fieldType = ProtocolDescriptor.StructDef.FieldType.U32;
            } else if (fieldTypeStr == "i64") {
                fieldType = ProtocolDescriptor.StructDef.FieldType.I64;
            } else if (fieldTypeStr == "u64") {
                fieldType = ProtocolDescriptor.StructDef.FieldType.U64;
            } else if (fieldTypeStr == "i16v") {
                fieldType = ProtocolDescriptor.StructDef.FieldType.I16V;
            } else if (fieldTypeStr == "u16v") {
                fieldType = ProtocolDescriptor.StructDef.FieldType.U16V;
            } else if (fieldTypeStr == "i32v") {
                fieldType = ProtocolDescriptor.StructDef.FieldType.I32V;
            } else if (fieldTypeStr == "u32v") {
                fieldType = ProtocolDescriptor.StructDef.FieldType.U32V;
            } else if (fieldTypeStr == "i64v") {
                fieldType = ProtocolDescriptor.StructDef.FieldType.I64V;
            } else if (fieldTypeStr == "u64v") {
                fieldType = ProtocolDescriptor.StructDef.FieldType.U64V;
            } else if (fieldTypeStr == "string") {
                fieldType = ProtocolDescriptor.StructDef.FieldType.String;
            } else if (fieldTypeStr == "bytes") {
                fieldType = ProtocolDescriptor.StructDef.FieldType.Bytes;
            } else if (fieldTypeStr == "bool") {
                fieldType = ProtocolDescriptor.StructDef.FieldType.Bool;
            } else {
                ProtocolDescriptor.ProtocolDef refProtoDef = null;
                string refDefName = "";

                string[] parts = fieldTypeStr.Split('.');
                if (parts.Length == 1) {
                    // in same file
                    refProtoDef = protoDef;
                    refDefName = parts[0];

                } else if (parts.Length == 2) {
                    // in other file
                    string refProtoDefName = parts[0];
                    if (this.descriptor.ImportedProtos.TryGetValue(
                            refProtoDefName, out refProtoDef) == false) {
                        PrintLineError(protoDef, element,
                            "protocol `{0}` is undefined", refProtoDefName);
                        return false;
                    }
                    refDefName = parts[1];
                } else {
                    PrintLineError(protoDef, element,
                        "type `{0}` is invalid", fieldTypeStr);
                    return false;
                }

                for (;;) {
                    // check is enum
                    ProtocolDescriptor.EnumDef refEnumDef = null;
                    if (refProtoDef.EnumNameIndex.TryGetValue(
                            refDefName, out refEnumDef)) {
                        fieldType = ProtocolDescriptor.StructDef.FieldType.Enum;
                        def.RefEnumDef = refEnumDef;
                        break;
                    }

                    // check is struct
                    ProtocolDescriptor.StructDef refStructDef = null;
                    if (refProtoDef.StructNameIndex.TryGetValue(
                            refDefName, out refStructDef)) {
                        fieldType = ProtocolDescriptor.StructDef.FieldType.Struct;
                        def.RefStructDef = refStructDef;
                        break;
                    }

                    PrintLineError(protoDef, element,
                        "type `{0}` is undefined", refDefName);
                    return false;
                }
            }

            if (def.Type == ProtocolDescriptor.StructDef.FieldType.List) {
                def.ListType = fieldType;
            } else {
                def.Type = fieldType;
            }

            // optional
            if (element.Name == "optional") {
                def.IsOptional = true;
                def.OptionalFieldIndex = structDef.OptionalFieldCount++;
            }

            structDef.Fields.Add(def);
            structDef.FieldNameIndex.Add(def.Name, def);

            return true;
        }

        private bool AddEnumMapDef(
            ProtocolDescriptor.ProtocolDef protoDef, XElement element)
        {
            // check name attr
            string name;
            {
                XAttribute attr = element.Attribute("name");
                if (attr == null) {
                    PrintLineError(protoDef, element,
                        "`enum_map` node must contain a `name` attribute");
                    return false;
                }
                name = attr.Value;
            }
            if (Regex.IsMatch(name, @"^[a-zA-Z_]\w*$") == false) {
                PrintLineError(protoDef, element,
                    "`enum_map` node `name` attribute is invalid");
                return false;
            }
            if (protoDef.EnumMapNameIndex.ContainsKey(name) ||
                protoDef.EnumNameIndex.ContainsKey(name) ||
                protoDef.StructNameIndex.ContainsKey(name)) {
                PrintLineError(protoDef, element,
                    "`enum_map` node `name` attribute duplicated");
                return false;
            }

            ProtocolDescriptor.EnumMapDef def =
                new ProtocolDescriptor.EnumMapDef();
            def.ParentRef = protoDef;
            def.Name = name;
            def.LineNumber = GetLineNumber(element);

            // parse items
            {
                IEnumerator<XElement> iter =
                    element.Elements().GetEnumerator();
                while (iter.MoveNext()) {
                    XElement childElement = iter.Current;

                    if (childElement.Name != "item") {
                        PrintLineError(protoDef, childElement,
                            "expect a `item` node");
                        return false;
                    }

                    if (AddEnumMapItemDef(protoDef, def,
                            childElement) == false) {
                        return false;
                    }
                }
            }

            protoDef.EnumMaps.Add(def);
            protoDef.EnumMapNameIndex.Add(def.Name, def);

            return true;
        }

        private bool AddEnumMapItemDef(
            ProtocolDescriptor.ProtocolDef protoDef,
            ProtocolDescriptor.EnumMapDef enumMapDef, XElement element)
        {
            // check name attr
            string name;
            {
                XAttribute attr = element.Attribute("name");
                if (attr == null) {
                    PrintLineError(protoDef, element,
                        "`item` node must contain a `name` attribute");
                    return false;
                }
                name = attr.Value;
            }
            if (Regex.IsMatch(name, @"^[a-zA-Z_]\w*$") == false) {
                PrintLineError(protoDef, element,
                    "`item` node `name` attribute is invalid");
                return false;
            }
            if (enumMapDef.ItemNameIndex.ContainsKey(name)) {
                PrintLineError(protoDef, element,
                    "`item` node `name` attribute duplicated");
                return false;
            }

            // check value attr
            string val = "";
            {
                XAttribute attr = element.Attribute("value");
                if (attr != null) {
                    val = attr.Value;
                }
            }

            // check struct attr
            string structVal = "";
            {
                XAttribute attr = element.Attribute("struct");
                if (attr != null) {
                    structVal = attr.Value;
                }
            }

            ProtocolDescriptor.EnumMapDef.ItemDef def =
                new ProtocolDescriptor.EnumMapDef.ItemDef();
            def.ParentRef = enumMapDef;
            def.Name = name;
            def.LineNumber = GetLineNumber(element);

            if (val == "") {
                // default
                def.Type = ProtocolDescriptor.EnumMapDef.ItemType.Default;
                if (enumMapDef.Items.Count == 0) {
                    def.IntValue = 0;
                } else {
                    def.IntValue = enumMapDef.Items[
                        enumMapDef.Items.Count - 1].IntValue + 1;
                }
            } else if (Regex.IsMatch(val, @"^[0-9]+$")) {
                // int
                def.Type = ProtocolDescriptor.EnumMapDef.ItemType.Int;
                def.IntValue = int.TryParse(val, out def.IntValue)
                    ? def.IntValue : 0;
            } else {
                // current enum
                ProtocolDescriptor.EnumMapDef.ItemDef refDef = null;
                if (enumMapDef.ItemNameIndex.TryGetValue(
                        val, out refDef) == false) {
                    PrintLineError(protoDef, element,
                        "enum_map item `{0}` is undefined", val);
                    return false;
                }

                def.Type =
                    ProtocolDescriptor.EnumMapDef.ItemType.CurrentEnumRef;
                def.IntValue = refDef.IntValue;
                def.RefEnumItemDef = refDef;
            }

            if (enumMapDef.Items.Count > 0 &&
                def.IntValue < enumMapDef.Items[
                    enumMapDef.Items.Count - 1].IntValue) {
                PrintLineError(protoDef, element,
                    "`item` node `value` attribute can not be " +
                    "less than previous one");
                return false;
            }

            if (structVal != "") {
                ProtocolDescriptor.ProtocolDef refProtoDef = null;
                string refDefName = "";

                string[] parts = structVal.Split('.');
                if (parts.Length == 1) {
                    // in same file
                    refProtoDef = protoDef;
                    refDefName = parts[0];

                } else if (parts.Length == 2) {
                    // in other file
                    string refProtoDefName = parts[0];
                    if (this.descriptor.ImportedProtos.TryGetValue(
                            refProtoDefName, out refProtoDef) == false) {
                        PrintLineError(protoDef, element,
                            "protocol `{0}` is undefined", refProtoDefName);
                        return false;
                    }
                    refDefName = parts[1];
                } else {
                    PrintLineError(protoDef, element,
                        "struct `{0}` is invalid", structVal);
                    return false;
                }

                ProtocolDescriptor.StructDef refStructDef = null;
                if (refProtoDef.StructNameIndex.TryGetValue(
                        refDefName, out refStructDef) == false) {
                    PrintLineError(protoDef, element,
                        "struct `{0}` is undefined", refDefName);
                    return false;
                }

                def.RefStructDef = refStructDef;

                if (enumMapDef.IdToStructIndex.ContainsKey(def.IntValue)) {
                    PrintLineError(protoDef, element,
                        "id `{0}` is already mapped to a struct",
                        def.IntValue);
                    return false;
                }
                if (enumMapDef.StructToIdIndex.ContainsKey(def.RefStructDef)) {
                    PrintLineError(protoDef, element,
                        "struct `{0}` is already mapped to a id",
                        def.RefStructDef.Name);
                    return false;
                }
                enumMapDef.IdToStructIndex.Add(def.IntValue, def.RefStructDef);
                enumMapDef.StructToIdIndex.Add(def.RefStructDef, def.IntValue);
            }

            enumMapDef.Items.Add(def);
            enumMapDef.ItemNameIndex.Add(def.Name, def);

            return true;
        }

        private void ProcessImportedProtocols(
            ProtocolDescriptor.ProtocolDef protoDef)
        {
            Dictionary<string, ProtocolDescriptor.ProtocolDef> usedProtos =
                new Dictionary<string, ProtocolDescriptor.ProtocolDef>();
            Dictionary<string, ProtocolDescriptor.ProtocolDef> enumRefProtos =
                new Dictionary<string, ProtocolDescriptor.ProtocolDef>();
            Dictionary<string, ProtocolDescriptor.ProtocolDef> structRefProtos =
                new Dictionary<string, ProtocolDescriptor.ProtocolDef>();
            Dictionary<string, ProtocolDescriptor.ProtocolDef> enumMapRefProtos =
                new Dictionary<string, ProtocolDescriptor.ProtocolDef>();

            // collect enum ref protocols
            for (int i = 0; i < protoDef.Enums.Count; ++i) {
                ProtocolDescriptor.EnumDef enumDef =
                    protoDef.Enums[i];

                for (int j = 0; j < enumDef.Items.Count; ++j) {
                    ProtocolDescriptor.EnumDef.ItemDef def =
                        enumDef.Items[j];
                    if (def.RefEnumItemDef != null) {
                        ProtocolDescriptor.ProtocolDef refProtoDef =
                            def.RefEnumItemDef.ParentRef.ParentRef;
                        usedProtos[refProtoDef.Name] = refProtoDef;
                        enumRefProtos[refProtoDef.Name] = refProtoDef;
                    }
                }
            }

            // collect struct ref protocols
            for (int i = 0; i < protoDef.Structs.Count; ++i) {
                ProtocolDescriptor.StructDef structDef =
                    protoDef.Structs[i];

                for (int j = 0; j < structDef.Fields.Count; ++j) {
                    ProtocolDescriptor.StructDef.FieldDef def =
                        structDef.Fields[j];
                    if (def.RefEnumDef != null) {
                        ProtocolDescriptor.ProtocolDef refProtoDef =
                            def.RefEnumDef.ParentRef;
                        usedProtos[refProtoDef.Name] = refProtoDef;
                        structRefProtos[refProtoDef.Name] = refProtoDef;
                    }
                    if (def.RefStructDef != null) {
                        ProtocolDescriptor.ProtocolDef refProtoDef =
                            def.RefStructDef.ParentRef;
                        usedProtos[refProtoDef.Name] = refProtoDef;
                        structRefProtos[refProtoDef.Name] = refProtoDef;
                    }
                }
            }

            // collect enum map ref protocols
            for (int i = 0; i < protoDef.EnumMaps.Count; ++i) {
                ProtocolDescriptor.EnumMapDef enumMapDef =
                    protoDef.EnumMaps[i];

                for (int j = 0; j < enumMapDef.Items.Count; ++j) {
                    ProtocolDescriptor.EnumMapDef.ItemDef def =
                        enumMapDef.Items[j];
                    if (def.RefEnumItemDef != null) {
                        ProtocolDescriptor.ProtocolDef refProtoDef =
                            def.RefEnumItemDef.ParentRef.ParentRef;
                        usedProtos[refProtoDef.Name] = refProtoDef;
                        enumMapRefProtos[refProtoDef.Name] = refProtoDef;
                    }
                    if (def.RefStructDef != null) {
                        ProtocolDescriptor.ProtocolDef refProtoDef =
                            def.RefStructDef.ParentRef;
                        usedProtos[refProtoDef.Name] = refProtoDef;
                        enumMapRefProtos[refProtoDef.Name] = refProtoDef;
                    }
                }
            }

            for (int i = 0; i < protoDef.Imports.Count; ++i) {
                ProtocolDescriptor.ImportDef importDef =
                    protoDef.Imports[i];

                // check imported protocol is used
                if (usedProtos.ContainsKey(importDef.ProtoDef.Name) == false) {
                    Console.Error.WriteLine(string.Format(
                        "warning:{0}:{1}: protocol `{2}` " +
                        "is not used but imported",
                        protoDef.FilePath, importDef.LineNumber,
                        importDef.Name));
                }

                if (enumRefProtos.ContainsKey(importDef.ProtoDef.Name)) {
                    importDef.IsRefByEnum = true;
                }
                if (structRefProtos.ContainsKey(importDef.ProtoDef.Name)) {
                    importDef.IsRefByStruct = true;
                }
                if (enumMapRefProtos.ContainsKey(importDef.ProtoDef.Name)) {
                    importDef.IsRefByEnumMap = true;
                }
            }
        }
    }
}
