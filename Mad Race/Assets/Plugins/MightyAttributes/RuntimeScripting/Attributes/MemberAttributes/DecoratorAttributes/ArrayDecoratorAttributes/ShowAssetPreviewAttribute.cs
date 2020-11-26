namespace MightyAttributes
{
    public class ShowAssetPreviewAttribute : BaseArrayDecoratorAttribute
    {
        public int Size { get; }
        public Align Align { get; }
        
        /// <summary>
        /// Draws a preview of the asset associated with the field.
        /// The field needs to be of type UnityEngine.Object (or be an array of this type), and it needs to have a preview texture.
        /// </summary>
        /// <param name="position">The position options of the decoration (default: After).</param>
        public ShowAssetPreviewAttribute(ArrayDecoratorPosition position = ArrayDecoratorPosition.After) : this(96, Align.Right, position)
        {
        }       
        
        /// <summary>
        /// Draws a preview of the asset associated with the field.
        /// The field needs to be of type UnityEngine.Object (or be an array of this type), and it needs to have a preview texture.
        /// </summary>
        /// <param name="positionCallback">Callback for the position options of the decoration.
        /// The callback type should be ArrayDecoratorPosition.</param>
        public ShowAssetPreviewAttribute(string positionCallback) : this(96, Align.Right, positionCallback)
        {
        }
        
        /// <summary>
        /// Draws a preview of the asset associated with the field.
        /// The field needs to be of type UnityEngine.Object (or be an array of this type), and it needs to have a preview texture.
        /// </summary>
        /// <param name="size">The size of the preview. It will be clamped by the maximum size of the preview texture.</param>
        /// <param name="align">The horizontal alignment of the preview.</param>
        public ShowAssetPreviewAttribute(int size, Align align) : base(ArrayDecoratorPosition.After)
        {
            Size = size;
            Align = align;
        }

        /// <summary>
        /// Draws a preview of the asset associated with the field.
        /// The field needs to be of type UnityEngine.Object (or be an array of this type), and it needs to have a preview texture.
        /// </summary>
        /// <param name="size">The size of the preview. It will be clamped by the maximum size of the preview texture.</param>
        /// <param name="align">The horizontal alignment of the preview.</param>
        /// <param name="position">The position options of the decoration.</param>
        public ShowAssetPreviewAttribute(int size, Align align, ArrayDecoratorPosition position) : base(position)
        {
            Size = size;
            Align = align;
        }

        /// <summary>
        /// Draws a preview of the asset associated with the field.
        /// The field needs to be of type UnityEngine.Object (or be an array of this type), and it needs to have a preview texture.
        /// </summary>
        /// <param name="size">The size of the preview. It will be clamped by the maximum size of the preview texture.</param>
        /// <param name="align">The horizontal alignment of the preview (default: Right).</param>
        /// <param name="positionCallback">Callback for the position options of the decoration.
        /// The callback type should be ArrayDecoratorPosition.</param>
        public ShowAssetPreviewAttribute(int size, Align align = Align.Right, string positionCallback = null) 
            : base(positionCallback, ArrayDecoratorPosition.After)
        {
            Size = size;
            Align = align;
        }
    }

    public class AssetPreviewAttribute : ShowAssetPreviewAttribute, IInheritDrawer
    {
        /// <summary>
        /// Draws a preview of the asset associated with the field.
        /// The field needs to be of type UnityEngine.Object (or be an array of this type), and it needs to have a preview texture.
        /// </summary>
        /// <param name="position">The position options of the decoration (default: After).</param>
        public AssetPreviewAttribute(ArrayDecoratorPosition position = ArrayDecoratorPosition.After) : base(position)
        {
        }       
        
        /// <summary>
        /// Draws a preview of the asset associated with the field.
        /// The field needs to be of type UnityEngine.Object (or be an array of this type), and it needs to have a preview texture.
        /// </summary>
        /// <param name="positionCallback">Callback for the position options of the decoration.
        /// The callback type should be ArrayDecoratorPosition.</param>
        public AssetPreviewAttribute(string positionCallback) : base(positionCallback)
        {
        }
        
        /// <summary>
        /// Draws a preview of the asset associated with the field.
        /// The field needs to be of type UnityEngine.Object (or be an array of this type), and it needs to have a preview texture.
        /// </summary>
        /// <param name="size">The size of the preview. It will be clamped by the maximum size of the preview texture.</param>
        /// <param name="align">The horizontal alignment of the preview.</param>
        public AssetPreviewAttribute(int size, Align align) : base(size, align)
        {
        }

        /// <summary>
        /// Draws a preview of the asset associated with the field.
        /// The field needs to be of type UnityEngine.Object (or be an array of this type), and it needs to have a preview texture.
        /// </summary>
        /// <param name="size">The size of the preview. It will be clamped by the maximum size of the preview texture.</param>
        /// <param name="align">The horizontal alignment of the preview.</param>
        /// <param name="position">The position options of the decoration.</param>
        public AssetPreviewAttribute(int size, Align align, ArrayDecoratorPosition position) : base(size, align, position)
        {
        }

        /// <summary>
        /// Draws a preview of the asset associated with the field.
        /// The field needs to be of type UnityEngine.Object (or be an array of this type), and it needs to have a preview texture.
        /// </summary>
        /// <param name="size">The size of the preview. It will be clamped by the maximum size of the preview texture.</param>
        /// <param name="align">The horizontal alignment of the preview (default: Right).</param>
        /// <param name="positionCallback">Callback for the position options of the decoration.
        /// The callback type should be ArrayDecoratorPosition.</param>
        public AssetPreviewAttribute(int size, Align align = Align.Right, string positionCallback = null) 
            : base(size, align, positionCallback)
        {
        }
    }
}