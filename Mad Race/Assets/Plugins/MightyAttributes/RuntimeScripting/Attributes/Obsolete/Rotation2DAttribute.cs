using System;

namespace MightyAttributes
{
    public class Rotation2DAttribute : BasePropertyDrawerAttribute
    {
        /// <summary>
        /// Uses a float slider to define the Z euler angle of a Quaternion.
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        [Obsolete("Suffers from Gimbal Lock.")]
        public Rotation2DAttribute(FieldOption options = FieldOption.Nothing) : base(options)
        {
        }
    }
}