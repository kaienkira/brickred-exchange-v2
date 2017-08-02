using System.Collections.Generic;

namespace Brickred.Exchange.Compiler
{
    public sealed class ProtocolDescriptor
    {
        public sealed class ImportDef
        {
            // link to parent define
            public ProtocolDef ParentRef = null;
            // import name
            public string Name = "";
            // define in line number
            public int LineNumber = 0;

            public ProtocolDef ProtoDef = null;

            // import is referenced by enum
            public bool IsRefByEnum = false;
            // import is referenced by struct
            public bool IsRefByStruct = false;
            // import is referenced by enum map
            public bool IsRefByEnumMap = false;
        }

        public sealed class NamespaceDef
        {
            // link to parent define
            public ProtocolDef ParentRef = null;
            // namespace for language
            public string Language = "";
            // define in line number
            public int LineNumber = 0;

            public string Namespace = "";
            public List<string> NamespaceParts = new List<string>();
        }

        public sealed class EnumDef
        {
            public enum ItemType
            {
                None = 0,
                Default,
                Int,
                CurrentEnumRef,
                OtherEnumRef,
            }

            public sealed class ItemDef
            {
                // link to parent define
                public EnumDef ParentRef = null;
                // enum item name
                public string Name = "";
                // define in line number
                public int LineNumber = 0;

                public ItemType Type = ItemType.None;
                public int IntValue = 0;
                public ItemDef RefEnumItemDef = null;
            }

            // link to parent define
            public ProtocolDef ParentRef = null;
            // enum name
            public string Name = "";
            // define in line number
            public int LineNumber = 0;

            // in file define order
            public List<ItemDef> Items = new List<ItemDef>();
            // ItemDef.Name -> ItemDef
            public Dictionary<string, ItemDef> ItemNameIndex =
                new Dictionary<string, ItemDef>();
        }

        public sealed class StructDef
        {
            public enum FieldType
            {
                None = 0,
                I8,
                U8,
                I16,
                U16,
                I32,
                U32,
                I64,
                U64,
                String,
                Bytes,
                Bool,
                Enum,
                Struct,
                List,
            }

            public sealed class FieldDef
            {
                // link to parent define
                public StructDef ParentRef = null;
                // field name
                public string Name = "";
                // define in line number
                public int LineNumber = 0;

                public FieldType Type = FieldType.None;
                public FieldType ListType = FieldType.None;
                public EnumDef RefEnumDef = null;
                public StructDef RefStructDef = null;
                public bool IsOptional = false;
                public int OptionalFieldIndex = 0;
            }

            // link to parent define
            public ProtocolDef ParentRef = null;
            // struct name
            public string Name = "";
            // define in line number
            public int LineNumber = 0;

            // in file define order
            public List<FieldDef> Fields = new List<FieldDef>();
            // FieldDef.Name -> FieldDef
            public Dictionary<string, FieldDef> FieldNameIndex =
                new Dictionary<string, FieldDef>();
            public int OptionalFieldCount = 0;
            public int OptionalByteCount = 0;
        }

        public sealed class EnumMapDef
        {
            public enum ItemType
            {
                None = 0,
                Default,
                Int,
                CurrentEnumRef,
            }

            public sealed class ItemDef
            {
                // link to parent define
                public EnumMapDef ParentRef = null;
                // enum map item name
                public string Name = "";
                // define in line number
                public int LineNumber = 0;

                public ItemType Type = ItemType.None;
                public int IntValue = 0;
                public ItemDef RefEnumItemDef = null;
                public StructDef RefStructDef = null;
            }

            // link to parent define
            public ProtocolDef ParentRef = null;
            // enum map name
            public string Name = "";
            // define in line number
            public int LineNumber = 0;

            // in file define order
            public List<ItemDef> Items = new List<ItemDef>();
            // ItemDef.Name -> ItemDef
            public Dictionary<string, ItemDef> ItemNameIndex =
                new Dictionary<string, ItemDef>();
            // StructId -> StructDef
            public Dictionary<int, StructDef> IdToStructIndex =
                new Dictionary<int, StructDef>();
            // StructDef -> StructId
            public Dictionary<StructDef, int> StructToIdIndex =
                new Dictionary<StructDef, int>();
        }

        public sealed class ProtocolDef
        {
            public string Name = "";
            public string FilePath = "";

            // import define
            // in file define order
            public List<ImportDef> Imports = new List<ImportDef>();
            // ImportDef.Name -> ImportDef
            public Dictionary<string, ImportDef> ImportNameIndex =
                new Dictionary<string, ImportDef>();

            // namespace define
            // language -> NamespaceDef
            public Dictionary<string, NamespaceDef> Namespaces =
                new Dictionary<string, NamespaceDef>();

            // enum define
            // in file define order
            public List<EnumDef> Enums = new List<EnumDef>();
            // EnumDef.Name -> EnumDef
            public Dictionary<string, EnumDef> EnumNameIndex =
                new Dictionary<string, EnumDef>();

            // struct define
            // in file define order
            public List<StructDef> Structs = new List<StructDef>();
            // StructDef.Name -> StructDef
            public Dictionary<string, StructDef> StructNameIndex =
                new Dictionary<string, StructDef>();

            // enum map define
            // in file define order
            public List<EnumMapDef> EnumMaps = new List<EnumMapDef>();
            // EnumMapDef.Name -> EnumMapDef
            public Dictionary<string, EnumMapDef> EnumMapNameIndex =
                new Dictionary<string, EnumMapDef>();
        }

        public ProtocolDef ProtoDef = null;
        // ProtocolDef.Name -> ProtocolDef
        public Dictionary<string, ProtocolDef> ImportedProtos =
            new Dictionary<string, ProtocolDef>();
    }
}
