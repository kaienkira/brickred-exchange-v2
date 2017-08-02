using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Brickred.Exchange.Compiler
{
    using EnumItemType = ProtocolDescriptor.EnumDef.ItemType;
    using EnumMapItemType = ProtocolDescriptor.EnumMapDef.ItemType;
    using FieldType = ProtocolDescriptor.StructDef.FieldType;

    public sealed class CppCodeGenerator : BaseCodeGenerator
    {
        private ProtocolDescriptor descriptor = null;
        private string newLineStr = "";

        public CppCodeGenerator()
        {
        }

        public override void Dispose()
        {
            this.newLineStr = "";

            if (this.descriptor != null) {
                this.descriptor = null;
            }
        }

        public override bool Generate(
            ProtocolDescriptor descriptor,
            string outputDir, NewLineType newLineType)
        {
            this.descriptor = descriptor;

            if (newLineType == NewLineType.Dos) {
                this.newLineStr = "\r\n";
            } else {
                this.newLineStr = "\n";
            }

            string headerFilePath = Path.Combine(
                outputDir, this.descriptor.ProtoDef.Name + ".h");
            string headerFileContent = GenerateHeaderFile();
            try {
                File.WriteAllText(headerFilePath, headerFileContent);
            } catch (Exception e) {
                Console.Error.WriteLine(string.Format(
                    "error: write file {0} failed: {1}",
                    headerFilePath, e.Message));
                return false;
            }

            string sourceFilePath = Path.Combine(
                outputDir, this.descriptor.ProtoDef.Name + ".cc");
            string sourceFileContent = GenerateSourceFile();
            try {
                File.WriteAllText(sourceFilePath, sourceFileContent);
            } catch (Exception e) {
                Console.Error.WriteLine(string.Format(
                    "error: write file {0} failed: {1}",
                    sourceFilePath, e.Message));
                return false;
            }

            return true;
        }

        private string GenerateHeaderFile()
        {
            string dontEditComment;
            string includeGuardStart;
            string includeGuardEnd;
            string includeFileDecl;
            string namespaceDeclStart;
            string namespaceDeclEnd;
            string classForwardDecl;
            List<string> declList = new List<string>();

            GetDontEditComment(
                out dontEditComment);
            GetNamespaceDecl(
                this.descriptor.ProtoDef,
                out namespaceDeclStart, out namespaceDeclEnd);
            GetHeaderFileIncludeGuard(
                this.descriptor.ProtoDef,
                out includeGuardStart, out includeGuardEnd);
            GetHeaderFileIncludeFileDecl(
                this.descriptor.ProtoDef,
                out includeFileDecl);
            GetHeaderFileClassForwardDecl(
                this.descriptor.ProtoDef,
                out classForwardDecl);

            ProtocolDescriptor.ProtocolDef protoDef =
                this.descriptor.ProtoDef;
            for (int i = 0; i < protoDef.Enums.Count; ++i) {
                string decl;
                GetHeaderFileEnumDecl(protoDef.Enums[i], out decl);
                declList.Add(decl);
            }
            for (int i = 0; i < protoDef.Structs.Count; ++i) {
                string decl;
                GetHeaderFileStructDecl(protoDef.Structs[i], out decl);
                declList.Add(decl);
            }
            for (int i = 0; i < protoDef.EnumMaps.Count; ++i) {
                string decl;
                GetHeaderFileEnumMapDecl(protoDef.EnumMaps[i], out decl);
                declList.Add(decl);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(dontEditComment);
            sb.Append(includeGuardStart);
            sb.Append(this.newLineStr);
            sb.Append(includeFileDecl);
            sb.Append(this.newLineStr);
            sb.Append(classForwardDecl);
            sb.Append(namespaceDeclStart);
            if (declList.Count > 0) {
                sb.Append(this.newLineStr);
            }
            sb.Append(string.Join(this.newLineStr, declList));
            if (declList.Count > 0) {
                sb.Append(this.newLineStr);
            }
            sb.Append(namespaceDeclEnd);
            if (namespaceDeclEnd != "") {
                sb.Append(this.newLineStr);
            }
            sb.Append(includeGuardEnd);

            return sb.ToString();
        }

        private string GenerateSourceFile()
        {
            string dontEditComment;
            string namespaceDeclStart;
            string namespaceDeclEnd;
            string includeFileDecl;
            List<string> implList = new List<string>();

            GetDontEditComment(
                out dontEditComment);
            GetNamespaceDecl(
                this.descriptor.ProtoDef,
                out namespaceDeclStart, out namespaceDeclEnd);
            GetSourceFileIncludeFileDecl(
                this.descriptor.ProtoDef,
                out includeFileDecl);

            ProtocolDescriptor.ProtocolDef protoDef =
                this.descriptor.ProtoDef;
            for (int i = 0; i < protoDef.Structs.Count; ++i) {
                string impl;
                GetSourceFileStructImpl(protoDef.Structs[i], out impl);
                implList.Add(impl);
            }
            for (int i = 0; i < protoDef.EnumMaps.Count; ++i) {
                string impl;
                GetSourceFileEnumMapImpl(protoDef.EnumMaps[i], out impl);
                implList.Add(impl);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(dontEditComment);
            sb.Append(includeFileDecl);
            sb.Append(this.newLineStr);
            sb.Append(namespaceDeclStart);
            if (implList.Count > 0) {
                sb.Append(this.newLineStr);
            }
            sb.Append(string.Join(this.newLineStr, implList));
            if (implList.Count > 0) {
                sb.Append(this.newLineStr);
            }
            sb.Append(namespaceDeclEnd);

            return sb.ToString();
        }

        private void GetDontEditComment(out string output)
        {
            output = string.Format(
                "/*{0}" +
                " * Generated by brickred exchange compiler.{0}" +
                " * Do not edit unless you are sure that you know what you are doing.{0}" +
                " */{0}",
                this.newLineStr);
        }

        private string GetEnumFullQualifiedName(
            ProtocolDescriptor.EnumDef enumDef)
        {
            ProtocolDescriptor.ProtocolDef protoDef = enumDef.ParentRef;
            ProtocolDescriptor.NamespaceDef namespaceDef = null;

            if (protoDef.Namespaces.TryGetValue(
                    "cpp", out namespaceDef) == false) {
                return enumDef.Name;
            } else {
                return string.Join("::", namespaceDef.NamespaceParts) +
                       "::" + enumDef.Name;
            }
        }

        private string GetStructFullQualifiedName(
            ProtocolDescriptor.StructDef structDef)
        {
            ProtocolDescriptor.ProtocolDef protoDef = structDef.ParentRef;
            ProtocolDescriptor.NamespaceDef namespaceDef = null;

            if (protoDef.Namespaces.TryGetValue(
                    "cpp", out namespaceDef) == false) {
                return structDef.Name;
            } else {
                return string.Join("::", namespaceDef.NamespaceParts) +
                       "::" + structDef.Name;
            }
        }

        private string GetEnumItemFullQualifiedName(
            ProtocolDescriptor.EnumDef.ItemDef itemDef)
        {
            ProtocolDescriptor.EnumDef enumDef = itemDef.ParentRef;
            ProtocolDescriptor.ProtocolDef protoDef = enumDef.ParentRef;
            ProtocolDescriptor.NamespaceDef namespaceDef = null;

            if (protoDef.Namespaces.TryGetValue(
                    "cpp", out namespaceDef) == false) {
                return enumDef.Name + "::" + itemDef.Name;
            } else {
                return string.Join("::", namespaceDef.NamespaceParts) +
                       "::" + enumDef.Name + "::" + itemDef.Name;
            }
        }

        private string GetCppType(
            ProtocolDescriptor.StructDef.FieldDef fieldDef)
        {
            FieldType checkType;
            if (fieldDef.Type == FieldType.List) {
                checkType = fieldDef.ListType;
            } else {
                checkType = fieldDef.Type;
            }

            string cppType = "";
            if (checkType == FieldType.I8) {
                cppType = "int8_t";
            } else if (checkType == FieldType.U8) {
                cppType = "uint8_t";
            } else if (checkType == FieldType.I16) {
                cppType = "int16_t";
            } else if (checkType == FieldType.U16) {
                cppType = "uint16_t";
            } else if (checkType == FieldType.I32) {
                cppType = "int32_t";
            } else if (checkType == FieldType.U32) {
                cppType = "uint32_t";
            } else if (checkType == FieldType.I64) {
                cppType = "int64_t";
            } else if (checkType == FieldType.U64) {
                cppType = "uint64_t";
            } else if (checkType == FieldType.String ||
                       checkType == FieldType.Bytes) {
                cppType = "std::string";
            } else if (checkType == FieldType.Bool) {
                cppType = "bool";
            } else if (checkType == FieldType.Enum) {
                cppType = GetEnumFullQualifiedName(
                    fieldDef.RefEnumDef) + "::type";
            } else if (checkType == FieldType.Struct) {
                cppType = GetStructFullQualifiedName(
                    fieldDef.RefStructDef);
            }

            if (fieldDef.Type == FieldType.List) {
                return string.Format("std::vector<{0}>", cppType);
            } else {
                return cppType;
            }
        }

        private void GetNamespaceDecl(
            ProtocolDescriptor.ProtocolDef protoDef,
            out string start, out string end)
        {
            start = "";
            end = "";

            ProtocolDescriptor.NamespaceDef namespaceDef = null;
            if (protoDef.Namespaces.TryGetValue(
                    "cpp", out namespaceDef) == false) {
                return;
            }

            StringBuilder sbStart = new StringBuilder();
            StringBuilder sbEnd = new StringBuilder();

            for (int i = 0; i < namespaceDef.NamespaceParts.Count; ++i) {
                string str = namespaceDef.NamespaceParts[i];

                sbStart.AppendFormat("namespace {0} {{{1}",
                    str, this.newLineStr);
                sbEnd.AppendFormat("}} // namespace {0}{1}",
                    str, this.newLineStr);
            }

            start = sbStart.ToString();
            end = sbEnd.ToString();
        }

        private void GetHeaderFileIncludeGuard(
            ProtocolDescriptor.ProtocolDef protoDef,
            out string start, out string end)
        {
            List<string> guardNameParts = new List<string>();

            guardNameParts.Add("BRICKRED_EXCHANGE_GENERATED");

            ProtocolDescriptor.NamespaceDef namespaceDef = null;
            if (protoDef.Namespaces.TryGetValue(
                    "cpp", out namespaceDef)) {
                guardNameParts.AddRange(namespaceDef.NamespaceParts);
            }

            guardNameParts.Add(Regex.Replace(
                protoDef.Name, @"[^\w]", "_"));
            guardNameParts.Add("H");

            string guardName = string.Join("_", guardNameParts).ToUpper();

            start = string.Format(
                "#ifndef {0}{1}" +
                "#define {0}{1}",
                guardName, this.newLineStr);
            end = string.Format(
                "#endif{0}",
                this.newLineStr);
        }

        private void GetHeaderFileIncludeFileDecl(
            ProtocolDescriptor.ProtocolDef protoDef, out string output)
        {
            bool useStdIntH = false;
            bool useStringH = false;
            bool useVectorH = false;
            bool useBrickredBaseStructH = false;

            {
                if (protoDef.Structs.Count > 0 ||
                    protoDef.EnumMaps.Count > 0) {
                    useBrickredBaseStructH = true;
                }

                for (int i = 0; i < protoDef.Structs.Count; ++i) {
                    ProtocolDescriptor.StructDef structDef =
                        protoDef.Structs[i];

                    for (int j = 0; j < structDef.Fields.Count; ++j) {
                        ProtocolDescriptor.StructDef.FieldDef fieldDef =
                            structDef.Fields[j];

                        if (fieldDef.IsOptional) {
                            useStdIntH = true;
                        }

                        FieldType checkType;
                        if (fieldDef.Type == FieldType.List) {
                            checkType = fieldDef.ListType;
                            useVectorH = true;
                        } else {
                            checkType = fieldDef.Type;
                        }

                        if (checkType == FieldType.I8 ||
                            checkType == FieldType.U8 ||
                            checkType == FieldType.I16 ||
                            checkType == FieldType.U16 ||
                            checkType == FieldType.I32 ||
                            checkType == FieldType.U32 ||
                            checkType == FieldType.I64 ||
                            checkType == FieldType.U64) {
                            useStdIntH = true;
                        } else if (checkType == FieldType.String ||
                                   checkType == FieldType.Bytes) {
                            useStringH = true;
                        }
                    }
                }
            }

            string systemHeaderDecl = "";
            {
                StringBuilder sb = new StringBuilder();

                if (useStdIntH) {
                    sb.AppendFormat("#include <stdint.h>{0}",
                        this.newLineStr);
                }

                sb.AppendFormat("#include <cstddef>{0}",
                    this.newLineStr);

                if (useStringH) {
                    sb.AppendFormat("#include <string>{0}",
                    this.newLineStr);
                }
                if (useVectorH) {
                    sb.AppendFormat("#include <vector>{0}",
                        this.newLineStr);
                }

                systemHeaderDecl = sb.ToString();
            }

            string customHeaderDecl = "";
            {
                StringBuilder sb = new StringBuilder();

                if (useBrickredBaseStructH) {
                    sb.AppendFormat(
                        "#include <brickred/exchange/base_struct.h>{0}",
                        this.newLineStr);
                }

                for (int i = 0; i < protoDef.Imports.Count; ++i) {
                    ProtocolDescriptor.ImportDef importDef =
                        protoDef.Imports[i];

                    if (importDef.IsRefByStruct == false &&
                        importDef.IsRefByEnumMap) {
                        continue;
                    }

                    sb.AppendFormat(
                        "#include \"{0}.h\"{1}",
                        importDef.ProtoDef.Name,
                        this.newLineStr);
                }

                customHeaderDecl = sb.ToString();
            }

            {
                StringBuilder sb = new StringBuilder();

                sb.Append(systemHeaderDecl);
                if (customHeaderDecl != "") {
                    sb.Append(this.newLineStr);
                }
                sb.Append(customHeaderDecl);

                output = sb.ToString();
            }
        }

        private void GetHeaderFileClassForwardDecl(
            ProtocolDescriptor.ProtocolDef protoDef, out string output)
        {
            StringBuilder sb = new StringBuilder();

            Dictionary<ProtocolDescriptor.ProtocolDef,
                       Dictionary<ProtocolDescriptor.StructDef, bool>>
                forwardDeclStructs = new Dictionary<
                    ProtocolDescriptor.ProtocolDef,
                    Dictionary<ProtocolDescriptor.StructDef, bool>>();

            for (int i = 0; i < protoDef.EnumMaps.Count; ++i) {
                ProtocolDescriptor.EnumMapDef enumMapDef =
                    protoDef.EnumMaps[i];

                for (int j = 0; j < enumMapDef.Items.Count; ++j) {
                    ProtocolDescriptor.EnumMapDef.ItemDef itemDef =
                        enumMapDef.Items[j];

                    if (itemDef.RefStructDef == null) {
                        continue;
                    }

                    ProtocolDescriptor.ProtocolDef refProtoDef =
                        itemDef.RefStructDef.ParentRef;
                    if (refProtoDef == protoDef) {
                        continue;
                    }

                    ProtocolDescriptor.ImportDef importDef = null;
                    if (protoDef.ImportNameIndex.TryGetValue(
                            refProtoDef.Name, out importDef) == false) {
                        continue;
                    }
                    if ((importDef.IsRefByStruct == false &&
                         importDef.IsRefByEnumMap) == false) {
                        continue;
                    }

                    if (forwardDeclStructs.ContainsKey(refProtoDef) == false) {
                        forwardDeclStructs[refProtoDef] =
                            new Dictionary<ProtocolDescriptor.StructDef, bool>();
                    }
                    forwardDeclStructs[refProtoDef][itemDef.RefStructDef] = true;
                }
            }

            Dictionary<ProtocolDescriptor.ProtocolDef,
                Dictionary<ProtocolDescriptor.StructDef, bool>>.Enumerator
                    iter = forwardDeclStructs.GetEnumerator();
            while (iter.MoveNext()) {
                ProtocolDescriptor.ProtocolDef refProtoDef =
                    iter.Current.Key;
                Dictionary<ProtocolDescriptor.StructDef, bool> structs =
                    iter.Current.Value;

                string namespaceDeclStart;
                string namespaceDeclEnd;
                GetNamespaceDecl(protoDef,
                    out namespaceDeclStart, out namespaceDeclEnd);

                sb.Append(namespaceDeclStart);
                if (structs.Count > 0) {
                    sb.Append(this.newLineStr);
                }

                Dictionary<ProtocolDescriptor.StructDef, bool>.Enumerator
                    iter2 = structs.GetEnumerator();
                while (iter2.MoveNext()) {
                    ProtocolDescriptor.StructDef structDef =
                        iter2.Current.Key;

                    sb.AppendFormat(
                        "class {0};{1}",
                        structDef.Name, this.newLineStr);
                }

                if (structs.Count > 0) {
                    sb.Append(this.newLineStr);
                }
                sb.Append(namespaceDeclEnd);

                if (namespaceDeclEnd != null) {
                    sb.Append(this.newLineStr);
                }
            }

            output = sb.ToString();
        }

        private void GetHeaderFileEnumDecl(
            ProtocolDescriptor.EnumDef enumDef, out string output)
        {
            StringBuilder sb = new StringBuilder();

            string start = string.Format(
                "struct {0} {{{1}" +
                "    enum type {{{1}",
                enumDef.Name, this.newLineStr);
            string end = string.Format(
                "    }};{0}" +
                "}};{0}",
                this.newLineStr);

            sb.Append(start);

            string indent = "        ";
            for (int i = 0; i < enumDef.Items.Count; ++i) {
                ProtocolDescriptor.EnumDef.ItemDef itemDef =
                    enumDef.Items[i];

                if (itemDef.Type == EnumItemType.Default) {
                    sb.AppendFormat("{0}{1},{2}",
                        indent, itemDef.Name, this.newLineStr);
                } else if (itemDef.Type == EnumItemType.Int) {
                    sb.AppendFormat("{0}{1} = {2},{3}",
                        indent, itemDef.Name,
                        itemDef.IntValue, this.newLineStr);
                } else if (itemDef.Type == EnumItemType.CurrentEnumRef) {
                    sb.AppendFormat("{0}{1} = {2},{3}",
                        indent, itemDef.Name,
                        itemDef.RefEnumItemDef.Name, this.newLineStr);
                } else if (itemDef.Type == EnumItemType.OtherEnumRef) {
                    sb.AppendFormat("{0}{1} = {2},{3}",
                        indent, itemDef.Name,
                        GetEnumItemFullQualifiedName(itemDef.RefEnumItemDef),
                        this.newLineStr);
                }
            }

            sb.Append(end);

            output = sb.ToString();
        }

        private void GetHeaderFileStructDecl(
            ProtocolDescriptor.StructDef structDef, out string output)
        {
            StringBuilder sb = new StringBuilder();

            string start = string.Format(
                "class {0} : public brickred::exchange::BaseStruct {{{1}" +
                "public:{1}" +
                "    {0}();{1}" +
                "    ~{0}();{1}" +
                "    void swap({0} &other);{1}" +
                "{1}" +
                "    static brickred::exchange::BaseStruct *create() {{" +
                " return new {0}(); }}{1}" +
                "    virtual {0} *clone() const {{" +
                " return new {0}(*this); }}{1}" +
                "    virtual int encode(char *buffer, size_t size) const;{1}" +
                "    virtual int decode(const char *buffer, size_t size);{1}",
                structDef.Name, this.newLineStr);
            string end = string.Format(
                "}};{0}", this.newLineStr);
            string optionalFuncDecl;
            string fieldDecl;

            GetHeaderFileStructDeclOptionalFuncDecl(
                structDef, out optionalFuncDecl);
            GetHeaderFileStructDeclFieldDecl(
                structDef, out fieldDecl);

            sb.Append(start);
            if (optionalFuncDecl != "") {
                sb.Append(this.newLineStr);
            }
            sb.Append(optionalFuncDecl);
            if (fieldDecl != "") {
                sb.Append(this.newLineStr);
            }
            sb.Append(fieldDecl);
            sb.Append(end);

            output = sb.ToString();
        }

        private void GetHeaderFileStructDeclOptionalFuncDecl(
            ProtocolDescriptor.StructDef structDef, out string output)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < structDef.Fields.Count; ++i) {
                ProtocolDescriptor.StructDef.FieldDef fieldDef =
                    structDef.Fields[i];

                if (fieldDef.IsOptional == false) {
                    continue;
                }

                string indent = "    ";
                string mask = string.Format("0x{0:x2}",
                    1 << fieldDef.OptionalFieldIndex % 8);

                string cppType = GetCppType(fieldDef);
                if (fieldDef.Type == FieldType.String ||
                    fieldDef.Type == FieldType.Bytes ||
                    fieldDef.Type == FieldType.List ||
                    fieldDef.Type == FieldType.Struct) {
                    cppType = string.Format("const {0} &", cppType);
                } else {
                    cppType = string.Format("{0} ", cppType);
                }

                sb.AppendFormat(
                    "{0}bool has_{1}() const {{ " +
                    "return _has_bits_[{3}] & {4}; }}{2}" +
                    "{0}void set_has_{1}() {{ " +
                    "_has_bits_[{3}] |= {4}; }}{2}" +
                    "{0}void clear_has_{1}() {{ " +
                    "_has_bits_[{3}] &= ~{4}; }}{2}" +
                    "{0}void set_{1}({5}value) {{ " +
                    "set_has_{1}(); this->{1} = value; }}{2}" +
                    "{2}",
                    indent, fieldDef.Name, this.newLineStr,
                    fieldDef.OptionalFieldIndex / 8, mask, cppType);
            }

            if (sb.Length > 0) {
                sb.AppendFormat(
                    "private:{0}" +
                    "    uint8_t _has_bits_[{1}];{0}",
                    this.newLineStr, structDef.OptionalByteCount);
            }

            output = sb.ToString();
        }

        private void GetHeaderFileStructDeclFieldDecl(
            ProtocolDescriptor.StructDef structDef, out string output)
        {
            StringBuilder sb = new StringBuilder();

            if (structDef.Fields.Count > 0) {
                sb.AppendFormat("public:{0}", this.newLineStr);
            }

            string indent = "    ";
            for (int i = 0; i < structDef.Fields.Count; ++i) {
                ProtocolDescriptor.StructDef.FieldDef fieldDef =
                    structDef.Fields[i];

                string cppType = GetCppType(fieldDef);
                sb.AppendFormat("{0}{1} {2};{3}",
                    indent, cppType, fieldDef.Name,
                    this.newLineStr);
            }

            output = sb.ToString();
        }

        private void GetHeaderFileEnumMapDecl(
            ProtocolDescriptor.EnumMapDef enumMapDef, out string output)
        {
            StringBuilder sb = new StringBuilder();

            string start = string.Format(
                "struct {0} {{{1}" +
                "    enum type {{{1}",
                enumMapDef.Name, this.newLineStr);
            string end = string.Format(
                "    }};{0}" +
                "{0}" +
                "    template<class T>{0}" +
                "    struct id;{0}" +
                "{0}" +
                "    static brickred::exchange::BaseStruct *create(int id);{0}" +
                "}};{0}",
                this.newLineStr);

            sb.Append(start);

            string indent = "        ";
            for (int i = 0; i < enumMapDef.Items.Count; ++i) {
                ProtocolDescriptor.EnumMapDef.ItemDef itemDef =
                    enumMapDef.Items[i];

                if (itemDef.Type == EnumMapItemType.Default) {
                    sb.AppendFormat("{0}{1},{2}",
                        indent, itemDef.Name, this.newLineStr);
                } else if (itemDef.Type == EnumMapItemType.Int) {
                    sb.AppendFormat("{0}{1} = {2},{3}",
                        indent, itemDef.Name,
                        itemDef.IntValue, this.newLineStr);
                } else if (itemDef.Type == EnumMapItemType.CurrentEnumRef) {
                    sb.AppendFormat("{0}{1} = {2},{3}",
                        indent, itemDef.Name,
                        itemDef.RefEnumItemDef.Name, this.newLineStr);
                }
            }

            sb.Append(end);

            string idTemplateDecl;
            GetHeaderFileEnumMapDeclIdTemplateDecl(
                enumMapDef, out idTemplateDecl);

            if (idTemplateDecl != "") {
                sb.Append(this.newLineStr);
            }
            sb.Append(idTemplateDecl);

            output = sb.ToString();
        }

        private void GetHeaderFileEnumMapDeclIdTemplateDecl(
            ProtocolDescriptor.EnumMapDef enumMapDef, out string output)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < enumMapDef.Items.Count; ++i) {
                ProtocolDescriptor.EnumMapDef.ItemDef itemDef =
                    enumMapDef.Items[i];

                if (itemDef.RefStructDef == null) {
                    continue;
                }

                sb.AppendFormat(
                    "template<>{0}" +
                    "struct {1}::id<{2}> {{{0}" +
                    "    static const int value = {3};{0}" +
                    "}};{0}",
                    this.newLineStr, enumMapDef.Name,
                    GetStructFullQualifiedName(itemDef.RefStructDef),
                    itemDef.Name);
            }

            output = sb.ToString();
        }

        private void GetSourceFileIncludeFileDecl(
            ProtocolDescriptor.ProtocolDef protoDef, out string output)
        {
            bool useCStringH = false;
            bool useAlgorithmH = false;
            bool useBrickredMacroInternalH = false;

            {
                if (protoDef.EnumMaps.Count > 0) {
                    // for std::lower_bound() in EnumMap::create()
                    useAlgorithmH = true;
                }

                for (int i = 0; i < protoDef.Structs.Count; ++i) {
                    ProtocolDescriptor.StructDef structDef =
                        protoDef.Structs[i];

                    if (structDef.OptionalFieldCount > 0) {
                        // for memset(_has_bits_)
                        useCStringH = true;
                        // for std::swap(_has_bits_)
                        useAlgorithmH = true;
                    }

                    if (structDef.Fields.Count > 0) {
                        useBrickredMacroInternalH = true;
                    }

                    for (int j = 0; j < structDef.Fields.Count; ++j) {
                        ProtocolDescriptor.StructDef.FieldDef fieldDef =
                            structDef.Fields[j];

                        if (fieldDef.Type != FieldType.List &&
                            fieldDef.Type != FieldType.Struct) {
                            // for std::swap(field)
                            useAlgorithmH = true;
                        }
                    }
                }
            }

            string systemHeaderDecl = "";
            {
                StringBuilder sb = new StringBuilder();

                if (useCStringH) {
                    sb.AppendFormat("#include <cstring>{0}",
                        this.newLineStr);
                }
                if (useAlgorithmH) {
                    sb.AppendFormat("#include <algorithm>{0}",
                        this.newLineStr);
                }

                systemHeaderDecl = sb.ToString();
            }

            string customHeaderDecl = "";
            {
                StringBuilder sb = new StringBuilder();

                if (useBrickredMacroInternalH) {
                    sb.AppendFormat(
                        "#include <brickred/exchange/macro_internal.h>{0}",
                        this.newLineStr);
                }

                for (int i = 0; i < protoDef.Imports.Count; ++i) {
                    ProtocolDescriptor.ImportDef importDef =
                        protoDef.Imports[i];

                    if ((importDef.IsRefByStruct == false &&
                        importDef.IsRefByEnumMap) == false) {
                        continue;
                    }

                    sb.AppendFormat(
                        "#include \"{0}.h\"{1}",
                        importDef.ProtoDef.Name,
                        this.newLineStr);
                }

                customHeaderDecl = sb.ToString();
            }

            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(
                    "#include \"{0}.h\"{1}",
                    protoDef.Name, this.newLineStr);
                if (systemHeaderDecl != "") {
                    sb.Append(this.newLineStr);
                }
                sb.Append(systemHeaderDecl);
                if (customHeaderDecl != "") {
                    sb.Append(this.newLineStr);
                }
                sb.Append(customHeaderDecl);

                output = sb.ToString();
            }
        }

        private void GetSourceFileStructImpl(
            ProtocolDescriptor.StructDef structDef, out string output)
        {
            StringBuilder sb = new StringBuilder();

            string constructorImpl;
            string destructorImpl;
            string swapFuncImpl;
            string encodeFuncImpl;
            string decodeFuncImpl;

            GetSourceFileStructImplConstructor(
                structDef, out constructorImpl);
            GetSourceFileStructImplDestructor(
                structDef, out destructorImpl);
            GetSourceFileStructImplSwapFunc(
                structDef, out swapFuncImpl);
            GetSourceFileStructImplEncodeFunc(
                structDef, out encodeFuncImpl);
            GetSourceFileStructImplDecodeFunc(
                structDef, out decodeFuncImpl);

            sb.Append(constructorImpl);
            sb.Append(this.newLineStr);
            sb.Append(destructorImpl);
            sb.Append(this.newLineStr);
            sb.Append(swapFuncImpl);
            sb.Append(this.newLineStr);
            sb.Append(encodeFuncImpl);
            sb.Append(this.newLineStr);
            sb.Append(decodeFuncImpl);

            output = sb.ToString();
        }

        private void GetSourceFileStructImplConstructor(
            ProtocolDescriptor.StructDef structDef, out string output)
        {
            StringBuilder sb = new StringBuilder();

            List<string> initListImpl = new List<string>();

            for (int i = 0; i < structDef.Fields.Count; ++i) {
                ProtocolDescriptor.StructDef.FieldDef fieldDef =
                    structDef.Fields[i];

                if (fieldDef.Type == FieldType.I8 ||
                    fieldDef.Type == FieldType.U8 ||
                    fieldDef.Type == FieldType.I16 ||
                    fieldDef.Type == FieldType.U16 ||
                    fieldDef.Type == FieldType.I32 ||
                    fieldDef.Type == FieldType.U32 ||
                    fieldDef.Type == FieldType.I64 ||
                    fieldDef.Type == FieldType.U64) {
                    initListImpl.Add(string.Format(
                        "    {0}(0)", fieldDef.Name));
                } else if (fieldDef.Type == FieldType.Bool) {
                    initListImpl.Add(string.Format(
                        "    {0}(false)", fieldDef.Name));
                } else if (fieldDef.Type == FieldType.Enum) {
                    if (fieldDef.RefEnumDef.Items.Count > 0) {
                        initListImpl.Add(string.Format(
                            "    {0}({1})", fieldDef.Name,
                            GetEnumItemFullQualifiedName(
                                fieldDef.RefEnumDef.Items[0])));
                    }
                }
            }

            string initList = "";
            if (initListImpl.Count > 0) {
                initList = string.Format(" :{0}", this.newLineStr) +
                    string.Join(string.Format(",{0}", this.newLineStr),
                        initListImpl);
            }

            string start = string.Format(
                "{0}::{0}(){1}{2}" +
                "{{{2}",
                structDef.Name, initList, this.newLineStr);
            string end = string.Format(
                "}}{0}", this.newLineStr);

            string hasBitsInitStatement = "";
            if (structDef.OptionalFieldCount > 0) {
                hasBitsInitStatement = string.Format(
                    "    ::memset(_has_bits_, 0, sizeof(_has_bits_));{0}",
                    this.newLineStr);
            }

            sb.Append(start);
            sb.Append(hasBitsInitStatement);
            sb.Append(end);

            output = sb.ToString();
        }

        private void GetSourceFileStructImplDestructor(
            ProtocolDescriptor.StructDef structDef, out string output)
        {
            output = string.Format(
                "{0}::~{0}(){1}" +
                "{{{1}" +
                "}}{1}",
                structDef.Name, this.newLineStr);
        }

        private void GetSourceFileStructImplSwapFunc(
            ProtocolDescriptor.StructDef structDef, out string output)
        {
            StringBuilder sb = new StringBuilder();

            string start = string.Format(
                "void {0}::swap({0} &other){1}" +
                "{{{1}",
                structDef.Name, this.newLineStr);
            string end = string.Format(
                "}}{0}", this.newLineStr);
            string indent = "    ";

            sb.Append(start);

            if (structDef.OptionalByteCount > 0) {
                sb.AppendFormat(
                    "{0}for (int i = 0; i < {1}; ++i) {{{2}" +
                    "{0}    std::swap(_has_bits_[i], other._has_bits_[i]);{2}" +
                    "{0}}}{2}" +
                    "{2}",
                    indent, structDef.OptionalByteCount,
                    this.newLineStr);
            }

            for (int i = 0; i < structDef.Fields.Count; ++i) {
                ProtocolDescriptor.StructDef.FieldDef fieldDef =
                    structDef.Fields[i];

                if (fieldDef.Type == FieldType.String ||
                    fieldDef.Type == FieldType.Bytes ||
                    fieldDef.Type == FieldType.List ||
                    fieldDef.Type == FieldType.Struct) {
                    sb.AppendFormat(
                        "{0}this->{1}.swap(other.{1});{2}",
                        indent, fieldDef.Name,
                        this.newLineStr);
                } else {
                    sb.AppendFormat(
                        "{0}std::swap(this->{1}, other.{1});{2}",
                        indent, fieldDef.Name,
                        this.newLineStr);
                }
            }

            sb.Append(end);

            output = sb.ToString();
        }

        private void GetSourceFileStructImplEncodeFunc(
            ProtocolDescriptor.StructDef structDef, out string output)
        {
            StringBuilder sb = new StringBuilder();

            string start = string.Format(
                "int {0}::encode(char *buffer, size_t size) const{1}" +
                "{{{1}",
                structDef.Name, this.newLineStr);
            string end = string.Format(
                "}}{0}", this.newLineStr);
            string indent = "    ";

            sb.Append(start);

            if (structDef.Fields.Count == 0) {
                sb.AppendFormat(
                    "{0}return 0;{1}",
                    indent, this.newLineStr);
            } else {
                sb.AppendFormat(
                    "{0}char *p = buffer;{1}" +
                    "{0}size_t left_bytes = size;{1}" +
                    "{1}",
                    indent, this.newLineStr);

                if (structDef.OptionalByteCount > 0) {
                    sb.AppendFormat(
                        "{0}for (int i = 0; i < {1}; ++i) {{{2}" +
                        "{0}    WRITE_INT8(_has_bits_[i]);{2}" +
                        "{0}}}{2}" +
                        "{2}",
                        indent, structDef.OptionalByteCount,
                        this.newLineStr);
                }

                for (int i = 0; i < structDef.Fields.Count; ++i) {
                    string writeStatement;
                    GetSourceFileStructImplEncodeFuncWriteStatement(
                        structDef.Fields[i], out writeStatement);
                    sb.Append(writeStatement);
                }

                sb.AppendFormat(
                    "{1}" +
                    "{0}return size - left_bytes;{1}",
                    indent, this.newLineStr);
            }

            sb.Append(end);

            output = sb.ToString();
        }

        private void GetSourceFileStructImplEncodeFuncWriteStatement(
            ProtocolDescriptor.StructDef.FieldDef fieldDef, out string output)
        {
            StringBuilder sb = new StringBuilder();

            string indent = "    ";
            string optionalCheckStart = "";
            string optionalCheckEnd = "";

            if (fieldDef.IsOptional) {
                optionalCheckStart = string.Format(
                    "{0}if (has_{1}()) {{{2}",
                    indent, fieldDef.Name, this.newLineStr);
                optionalCheckEnd = string.Format(
                    "{0}}}{1}",
                    indent, this.newLineStr);
                indent += "    ";
            }

            sb.Append(optionalCheckStart);

            FieldType checkType;
            if (fieldDef.Type == FieldType.List) {
                checkType = fieldDef.ListType;
            } else {
                checkType = fieldDef.Type;
            }
            bool isList = (fieldDef.Type == FieldType.List);

            if (checkType == FieldType.I8 ||
                checkType == FieldType.U8 ||
                checkType == FieldType.Bool) {
                if (isList) {
                    sb.AppendFormat(
                        "{0}WRITE_LIST(this->{1}, WRITE_INT8);{2}",
                        indent, fieldDef.Name, this.newLineStr);
                } else {
                    sb.AppendFormat(
                        "{0}WRITE_INT8(this->{1});{2}",
                        indent, fieldDef.Name, this.newLineStr);
                }
            } else if (checkType == FieldType.I16 ||
                       checkType == FieldType.U16) {
                if (isList) {
                    sb.AppendFormat(
                        "{0}WRITE_LIST(this->{1}, WRITE_INT16);{2}",
                        indent, fieldDef.Name, this.newLineStr);
                } else {
                    sb.AppendFormat(
                        "{0}WRITE_INT16(this->{1});{2}",
                        indent, fieldDef.Name, this.newLineStr);
                }
            } else if (checkType == FieldType.I32 ||
                       checkType == FieldType.U32) {
                if (isList) {
                    sb.AppendFormat(
                        "{0}WRITE_LIST(this->{1}, WRITE_INT32);{2}",
                        indent, fieldDef.Name, this.newLineStr);
                } else {
                    sb.AppendFormat(
                        "{0}WRITE_INT32(this->{1});{2}",
                        indent, fieldDef.Name, this.newLineStr);
                }
            } else if (checkType == FieldType.I64 ||
                       checkType == FieldType.U64) {
                if (isList) {
                    sb.AppendFormat(
                        "{0}WRITE_LIST(this->{1}, WRITE_INT64);{2}",
                        indent, fieldDef.Name, this.newLineStr);
                } else {
                    sb.AppendFormat(
                        "{0}WRITE_INT64(this->{1});{2}",
                        indent, fieldDef.Name, this.newLineStr);
                }
            } else if (checkType == FieldType.String ||
                       checkType == FieldType.Bytes) {
                if (isList) {
                    sb.AppendFormat(
                        "{0}WRITE_LIST(this->{1}, WRITE_STRING);{2}",
                        indent, fieldDef.Name, this.newLineStr);
                } else {
                    sb.AppendFormat(
                        "{0}WRITE_STRING(this->{1});{2}",
                        indent, fieldDef.Name, this.newLineStr);
                }
            } else if (checkType == FieldType.Enum) {
                if (isList) {
                    sb.AppendFormat(
                        "{0}WRITE_LIST(this->{1}, WRITE_ENUM);{2}",
                        indent, fieldDef.Name, this.newLineStr);
                } else {
                    sb.AppendFormat(
                        "{0}WRITE_ENUM(this->{1});{2}",
                        indent, fieldDef.Name, this.newLineStr);
                }
            } else if (checkType == FieldType.Struct) {
                if (isList) {
                    sb.AppendFormat(
                        "{0}WRITE_LIST(this->{1}, WRITE_STRUCT);{2}",
                        indent, fieldDef.Name, this.newLineStr);
                } else {
                    sb.AppendFormat(
                        "{0}WRITE_STRUCT(this->{1});{2}",
                        indent, fieldDef.Name, this.newLineStr);
                }
            }

            sb.Append(optionalCheckEnd);

            output = sb.ToString();
        }

        private void GetSourceFileStructImplDecodeFunc(
            ProtocolDescriptor.StructDef structDef, out string output)
        {
            StringBuilder sb = new StringBuilder();

            string start = string.Format(
                "int {0}::decode(const char *buffer, size_t size){1}" +
                "{{{1}",
                structDef.Name, this.newLineStr);
            string end = string.Format(
                "}}{0}", this.newLineStr);
            string indent = "    ";

            sb.Append(start);

            if (structDef.Fields.Count == 0) {
                sb.AppendFormat(
                    "{0}return 0;{1}",
                    indent, this.newLineStr);
            } else {
                sb.AppendFormat(
                    "{0}const char *p = buffer;{1}" +
                    "{0}size_t left_bytes = size;{1}" +
                    "{1}",
                    indent, this.newLineStr);

                if (structDef.OptionalByteCount > 0) {
                    sb.AppendFormat(
                        "{0}for (int i = 0; i < {1}; ++i) {{{2}" +
                        "{0}    READ_INT8(_has_bits_[i]);{2}" +
                        "{0}}}{2}" +
                        "{2}",
                        indent, structDef.OptionalByteCount,
                        this.newLineStr);
                }

                for (int i = 0; i < structDef.Fields.Count; ++i) {
                    string readStatement;
                    GetSourceFileStructImplDecodeFuncReadStatement(
                        structDef.Fields[i], out readStatement);
                    sb.Append(readStatement);
                }

                sb.AppendFormat(
                    "{1}" +
                    "{0}return size - left_bytes;{1}",
                    indent, this.newLineStr);
            }

            sb.Append(end);

            output = sb.ToString();
        }

        private void GetSourceFileStructImplDecodeFuncReadStatement(
            ProtocolDescriptor.StructDef.FieldDef fieldDef, out string output)
        {
            StringBuilder sb = new StringBuilder();

            string indent = "    ";
            string optionalCheckStart = "";
            string optionalCheckEnd = "";

            if (fieldDef.IsOptional) {
                optionalCheckStart = string.Format(
                    "{0}if (has_{1}()) {{{2}",
                    indent, fieldDef.Name, this.newLineStr);
                optionalCheckEnd = string.Format(
                    "{0}}}{1}",
                    indent, this.newLineStr);
                indent += "    ";
            }

            sb.Append(optionalCheckStart);

            FieldType checkType;
            if (fieldDef.Type == FieldType.List) {
                checkType = fieldDef.ListType;
            } else {
                checkType = fieldDef.Type;
            }
            bool isList = (fieldDef.Type == FieldType.List);

            if (checkType == FieldType.I8 ||
                checkType == FieldType.U8 ||
                checkType == FieldType.Bool) {
                if (isList) {
                    string cppType = "";
                    if (checkType == FieldType.I8) {
                        cppType = "int8_t";
                    } else if (checkType == FieldType.U8) {
                        cppType = "uint8_t";
                    } else if (checkType == FieldType.Bool) {
                        cppType = "bool";
                    }
                    sb.AppendFormat(
                        "{0}READ_LIST(this->{1}, READ_INT8, {2});{3}",
                        indent, fieldDef.Name, cppType, this.newLineStr);
                } else {
                    sb.AppendFormat(
                        "{0}READ_INT8(this->{1});{2}",
                        indent, fieldDef.Name, this.newLineStr);
                }
            } else if (checkType == FieldType.I16 ||
                       checkType == FieldType.U16) {
                if (isList) {
                    string cppType = "";
                    if (checkType == FieldType.I16) {
                        cppType = "int16_t";
                    } else if (checkType == FieldType.U16) {
                        cppType = "uint16_t";
                    }
                    sb.AppendFormat(
                        "{0}READ_LIST(this->{1}, READ_INT16, {2});{3}",
                        indent, fieldDef.Name, cppType, this.newLineStr);
                } else {
                    sb.AppendFormat(
                        "{0}READ_INT16(this->{1});{2}",
                        indent, fieldDef.Name, this.newLineStr);
                }
            } else if (checkType == FieldType.I32 ||
                       checkType == FieldType.U32) {
                if (isList) {
                    string cppType = "";
                    if (checkType == FieldType.I32) {
                        cppType = "int32_t";
                    } else if (checkType == FieldType.U32) {
                        cppType = "uint32_t";
                    }
                    sb.AppendFormat(
                        "{0}READ_LIST(this->{1}, READ_INT32, {2});{3}",
                        indent, fieldDef.Name, cppType, this.newLineStr);
                } else {
                    sb.AppendFormat(
                        "{0}READ_INT32(this->{1});{2}",
                        indent, fieldDef.Name, this.newLineStr);
                }
            } else if (checkType == FieldType.I64 ||
                       checkType == FieldType.U64) {
                if (isList) {
                    string cppType = "";
                    if (checkType == FieldType.I64) {
                        cppType = "int64_t";
                    } else if (checkType == FieldType.U64) {
                        cppType = "uint64_t";
                    }
                    sb.AppendFormat(
                        "{0}READ_LIST(this->{1}, READ_INT64, {2});{3}",
                        indent, fieldDef.Name, cppType, this.newLineStr);
                } else {
                    sb.AppendFormat(
                        "{0}READ_INT64(this->{1});{2}",
                        indent, fieldDef.Name, this.newLineStr);
                }
            } else if (checkType == FieldType.String ||
                       checkType == FieldType.Bytes) {
                if (isList) {
                    sb.AppendFormat(
                        "{0}READ_LIST(this->{1}, READ_STRING, std::string);{2}",
                        indent, fieldDef.Name, this.newLineStr);
                } else {
                    sb.AppendFormat(
                        "{0}READ_STRING(this->{1});{2}",
                        indent, fieldDef.Name, this.newLineStr);
                }
            } else if (checkType == FieldType.Enum) {
                if (isList) {
                    sb.AppendFormat(
                        "{0}READ_ENUM_LIST(this->{1}, {2}::type);{3}",
                        indent, fieldDef.Name,
                        GetEnumFullQualifiedName(fieldDef.RefEnumDef),
                        this.newLineStr);
                } else {
                    sb.AppendFormat(
                        "{0}READ_ENUM(this->{1}, {2}::type);{3}",
                        indent, fieldDef.Name,
                        GetEnumFullQualifiedName(fieldDef.RefEnumDef),
                        this.newLineStr);
                }
            } else if (checkType == FieldType.Struct) {
                if (isList) {
                    sb.AppendFormat(
                        "{0}READ_LIST(this->{1}, READ_STRUCT, {2});{3}",
                        indent, fieldDef.Name,
                        GetStructFullQualifiedName(fieldDef.RefStructDef),
                        this.newLineStr);
                } else {
                    sb.AppendFormat(
                        "{0}READ_STRUCT(this->{1});{2}",
                        indent, fieldDef.Name, this.newLineStr);
                }
            }

            sb.Append(optionalCheckEnd);

            output = sb.ToString();
        }

        private void GetSourceFileEnumMapImpl(
            ProtocolDescriptor.EnumMapDef enumMapDef, out string output)
        {
            StringBuilder sb = new StringBuilder();

            string createFuncImpl;

            GetSourceFileEnumMapImplCreateFunc(
                enumMapDef, out createFuncImpl);

            sb.Append(createFuncImpl);

            output = sb.ToString();
        }

        private void GetSourceFileEnumMapImplCreateFunc(
            ProtocolDescriptor.EnumMapDef enumMapDef, out string output)
        {
            StringBuilder sbIdList = new StringBuilder();
            StringBuilder sbFuncList = new StringBuilder();
            string indent = "        ";

            for (int i = 0; i < enumMapDef.Items.Count; ++i) {
                ProtocolDescriptor.EnumMapDef.ItemDef itemDef =
                    enumMapDef.Items[i];

                if (itemDef.RefStructDef == null) {
                    continue;
                }

                sbIdList.AppendFormat("{0}{1},{2}",
                    indent, itemDef.Name, this.newLineStr);
                sbFuncList.AppendFormat("{0}&{1}::create,{2}",
                    indent, GetStructFullQualifiedName(itemDef.RefStructDef),
                    this.newLineStr);
            }

            output = string.Format(
                "brickred::exchange::BaseStruct *{0}::create(int id){1}" +
                "{{{1}" +
                "    static int id_list[] = {{{1}" +
                "{2}" +
                "    }};{1}" +
                "{1}" +
                "    static brickred::exchange::BaseStruct::CreateFunc " +
                "create_func_list[] = {{{1}" +
                "{3}" +
                "    }};{1}" +
                "{1}" +
                "    static const int *begin = id_list;{1}" +
                "    static const int *end = id_list + " +
                "sizeof(id_list) / sizeof(int);{1}" +
                "{1}" +
                "    const int *ret = std::lower_bound(begin, end, id);{1}" +
                "    if (ret == end || *ret != id) {{{1}" +
                "        return NULL;{1}" +
                "    }} else {{{1}" +
                "        return create_func_list[ret - begin]();{1}" +
                "    }}{1}" +
                "}}{1}",
                enumMapDef.Name, this.newLineStr,
                sbIdList.ToString(), sbFuncList.ToString());
        }
    }
}
