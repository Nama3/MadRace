using System;

namespace MightyAttributes
{
    public enum HelpBoxType : byte
    {
        Info,
        Warning,
        Error
    }

    public enum ColorValue : byte
    {
        Default,

        Red,
        Orange,
        Yellow,
        Green,
        Blue,
        Purple,
        Pink,

        White,
        Gray,
        DarkGray,
        Black,

        Brightest,
        Brighter,
        Bright,
        Dark,
        Darker,
        Darkest, 
        
        SofterContrast,
        SoftContrast,
        Contrast,
        HardContrast
    }

    [Flags]
    public enum Indent : short
    {
        All = -1,

        Nothing = 0,

        Label = 1,
        Content = 1 << 1
    }

    #region Options

    [Flags]
    public enum FieldOption : short
    {
        Nothing = 0,
        All = -1,

        HideLabel = 1,
        BoldLabel = 1 << 1
    }

    [Flags]
    public enum NestOption : short
    {
        Nothing = 0,
        All = -1,

        HideLabel = FieldOption.HideLabel,
        BoldLabel = FieldOption.BoldLabel,

        DontFold = 1 << 2,
        DontIndent = 1 << 3,

        ContentOnly = HideLabel | DontIndent
    }

    [Flags]
    public enum ArrayOption : short
    {
        Nothing = 0,
        All = -1,

        HideLabel = FieldOption.HideLabel,
        BoldLabel = FieldOption.BoldLabel,

        HideSizeField = 1 << 2,
        DisableSizeField = 1 << 3,
        
        HideElementLabel = 1 << 4,
        LabelInHeader = 1 << 5,
        
        DontFold = 1 << 6,
        DontIndent = 1 << 7,

        ContentOnly = HideLabel | HideSizeField | DontIndent
    }

    [Flags]
    public enum EditorFieldOption : short
    {
        Nothing = 0,
        All = -1,

        HideLabel = FieldOption.HideLabel,
        BoldLabel = FieldOption.BoldLabel,

        DontFold = NestOption.DontFold,

        Deserialize = 1 << 3,
        Serialize = 1 << 4,

        Hide = 1 << 5,
        Asset = 1 << 6,

        HideDeserialize = Deserialize | Hide,
        HideDontFold = Hide | DontFold,
        HideAsset = Hide | Asset,

        Default = Deserialize | Serialize,
        HideDefault = Hide | Default,
        AssetDefault = Asset | Default
    }

    #endregion /Options
    
    [Flags]
    public enum HideStatus : short
    {
        Nothing = 0,
        All = -1,

        SerializedFields = 1,
        ScriptField = 1 << 1
    }

    #region Positions

    public enum Orientation : byte
    {
        Horizontal,
        Vertical
    }

    public enum Align : byte
    {
        Left,
        Center,
        Right
    }

    [Flags]
    public enum DecoratorPosition : short
    {
        Nothing = 0,
        All = -1,

        Before = 1,
        After = 1 << 1,

        Horizontal = 1 << 2,

        BeforeHorizontal = Before | Horizontal,
        AfterHorizontal = After | Horizontal,

        Wrap = Before | After
    }
    
    [Flags]
    public enum ArrayDecoratorPosition : short
    {
        Nothing = 0,
        All = -1,

        Before = DecoratorPosition.Before,
        After = DecoratorPosition.After,

        Horizontal = DecoratorPosition.Horizontal,

        BeforeHorizontal = DecoratorPosition.BeforeHorizontal,
        AfterHorizontal = DecoratorPosition.AfterHorizontal,
        
        BeforeElements = 1 << 3,
        BetweenElements = 1 << 4,
        AfterElements = 1 << 5,

        BeforeHeader = 1 << 6,
        AfterHeader = 1 << 7,
        AfterHeaderFoldout = 1 << 8,

        Wrap = DecoratorPosition.Wrap,
        
        WrapElements = BeforeElements | AfterElements,
        WrapHeader = BeforeHeader | AfterHeader,
        WrapHeaderFoldout = BeforeHeader | AfterHeaderFoldout,
        WrapArray = Wrap | AfterHeaderFoldout | BetweenElements
    }

    #endregion /Positions

    public static class MightyEnumsUtilities
    {
        public static bool Contains(this FieldOption option, FieldOption flag) => (option & flag) != 0;
        public static bool ContainsExact(this FieldOption option, FieldOption flag) => (option & flag) == flag;

        public static bool Contains(this NestOption option, NestOption flag) => (option & flag) != 0;
        public static bool ContainsExact(this NestOption option, NestOption flag) => (option & flag) == flag;

        public static bool Contains(this ArrayOption option, ArrayOption flag) => (option & flag) != 0;
        public static bool ContainsExact(this ArrayOption option, ArrayOption flag) => (option & flag) == flag;

        public static bool Contains(this EditorFieldOption option, EditorFieldOption flag) => (option & flag) != 0;
        public static bool ContainsExact(this EditorFieldOption option, EditorFieldOption flag) => (option & flag) == flag;
        
        public static bool Contains(this HideStatus hideStatus, HideStatus flag) => (hideStatus & flag) != 0;
        public static bool ContainsExact(this HideStatus hideStatus, HideStatus flag) => (hideStatus & flag) == flag;
        
        public static bool Contains(this DecoratorPosition position, DecoratorPosition flag) => (position & flag) != 0;
        public static bool ContainsExact(this DecoratorPosition position, DecoratorPosition flag) => (position & flag) == flag;
        
        public static bool Contains(this ArrayDecoratorPosition position, ArrayDecoratorPosition flag) => (position & flag) != 0;
        public static bool ContainsExact(this ArrayDecoratorPosition position, ArrayDecoratorPosition flag) => (position & flag) == flag;
    }
}