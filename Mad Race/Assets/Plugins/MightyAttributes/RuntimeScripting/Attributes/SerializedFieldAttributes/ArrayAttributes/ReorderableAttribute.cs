namespace MightyAttributes
{
    public class ReorderableListAttribute : BaseArrayAttribute
    {
        public bool DrawButtons { get; }
        public bool Draggable { get; }

        /// <summary>
        /// Provides a handy field that allows you to reorder arrays.
        /// </summary>
        /// <param name="drawButtons">Choose whether or not to draw the “+” and “–“ buttons at the bottom right of the array (default: true).</param>
        /// <param name="draggable">Choose whether or not the items of the array can be dragged (default: true).</param>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public ReorderableListAttribute(bool drawButtons = true, bool draggable = true, ArrayOption options = ArrayOption.Nothing)
            : base(options)
        {
            DrawButtons = drawButtons;
            Draggable = draggable;
        }
        
        /// <summary>
        /// Provides a handy field that allows you to reorder arrays.
        /// </summary>
        /// <param name="drawButtons">Choose whether or not to draw the “+” and “–“ buttons at the bottom right of the array (default: true).</param>
        /// <param name="draggable">Choose whether or not the items of the array can be dragged (default: true).</param>
        /// <param name="optionsCallback">Callback for the drawing options of the field.</param>
        public ReorderableListAttribute(bool drawButtons, bool draggable, string optionsCallback) : base(optionsCallback)
        {
            DrawButtons = drawButtons;
            Draggable = draggable;
        }

        /// <summary>
        /// Provides a handy field that allows you to reorder arrays.
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public ReorderableListAttribute(ArrayOption options) : base(options)
        {
            DrawButtons = true;
            Draggable = true;
        }

        /// <summary>
        /// Provides a handy field that allows you to reorder arrays.
        /// </summary>
        /// <param name="optionsCallback">Callback for the drawing options of the field.</param>
        public ReorderableListAttribute(string optionsCallback) : base(optionsCallback)
        {
            DrawButtons = true;
            Draggable = true;
        }
    }
    
    public class ReorderableArrayAttribute : ReorderableListAttribute, IInheritDrawer
    {
        /// <summary>
        /// Provides a handy field that allows you to reorder arrays.
        /// </summary>
        /// <param name="drawButtons">Choose whether or not to draw the “+” and “–“ buttons at the bottom right of the array (default: true).</param>
        /// <param name="draggable">Choose whether or not the items of the array can be dragged (default: true).</param>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public ReorderableArrayAttribute(bool drawButtons = true, bool draggable = true, ArrayOption options = ArrayOption.Nothing)
            : base(drawButtons, draggable, options)
        {
        }
        
        /// <summary>
        /// Provides a handy field that allows you to reorder arrays.
        /// </summary>
        /// <param name="drawButtons">Choose whether or not to draw the “+” and “–“ buttons at the bottom right of the array (default: true).</param>
        /// <param name="draggable">Choose whether or not the items of the array can be dragged (default: true).</param>
        /// <param name="optionsCallback">Callback for the drawing options of the field.</param>
        public ReorderableArrayAttribute(bool drawButtons, bool draggable, string optionsCallback) 
            : base(drawButtons, draggable, optionsCallback)
        {
        }

        /// <summary>
        /// Provides a handy field that allows you to reorder arrays.
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public ReorderableArrayAttribute(ArrayOption options) : base(options)
        {
        }

        /// <summary>
        /// Provides a handy field that allows you to reorder arrays.
        /// </summary>
        /// <param name="optionsCallback">Callback for the drawing options of the field.</param>
        public ReorderableArrayAttribute(string optionsCallback) : base(optionsCallback)
        {
        }
    }

    public class ReorderableAttribute : ReorderableListAttribute, IInheritDrawer
    {
        /// <summary>
        /// Provides a handy field that allows you to reorder arrays.
        /// </summary>
        /// <param name="drawButtons">Choose whether or not to draw the “+” and “–“ buttons at the bottom right of the array (default: true).</param>
        /// <param name="draggable">Choose whether or not the items of the array can be dragged (default: true).</param>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public ReorderableAttribute(bool drawButtons = true, bool draggable = true, ArrayOption options = ArrayOption.Nothing)
            : base(drawButtons, draggable, options)
        {
        }
        
        /// <summary>
        /// Provides a handy field that allows you to reorder arrays.
        /// </summary>
        /// <param name="drawButtons">Choose whether or not to draw the “+” and “–“ buttons at the bottom right of the array (default: true).</param>
        /// <param name="draggable">Choose whether or not the items of the array can be dragged (default: true).</param>
        /// <param name="optionsCallback">Callback for the drawing options of the field.
        /// The callback type should be ArrayOption.</param>
        public ReorderableAttribute(bool drawButtons, bool draggable, string optionsCallback)
            : base(drawButtons, draggable, optionsCallback)
        {
        }

        /// <summary>
        /// Provides a handy field that allows you to reorder arrays.
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public ReorderableAttribute(ArrayOption options) : base(options)
        {
        }

        /// <summary>
        /// Provides a handy field that allows you to reorder arrays.
        /// </summary>
        /// <param name="optionsCallback">Callback for the drawing options of the field.
        /// The callback type should be ArrayOption.</param>
        public ReorderableAttribute(string optionsCallback) : base(optionsCallback)
        {
        }
    }
}