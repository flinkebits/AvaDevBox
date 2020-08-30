﻿using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaDevBox.MarkupExtensions
{
    /// <summary>
    /// Use this markup extension to use enums as data source
    /// where item collections are requested. I.e. <see cref="ComboBox"/>.
    /// </summary>
    /// <example>
    ///     <ComboBox Items="{Binding Source={adb:EnumBindingSource {x:Type adb:ConnectionLinePlacement}}}"/>
    /// </example>
    public class EnumBindingSource : AvaloniaObject /*: MarkupExtension*/
    {
        private Type _enumType;
        public Type EnumType
        {
            get { return this._enumType; }
            set
            {
                if (value != this._enumType)
                {
                    if (null != value)
                    {
                        Type enumType = Nullable.GetUnderlyingType(value) ?? value;

                        if (!enumType.IsEnum)
                            throw new ArgumentException("Type must be for an Enum.");
                    }

                    this._enumType = value;
                }
            }
        }

        public EnumBindingSource() { }

        public EnumBindingSource(Type enumType)
        {
            this.EnumType = enumType;
        }

        public  Array ProvideValue(IServiceProvider serviceProvider)
        {
            if (null == this._enumType)
                throw new InvalidOperationException("The EnumType must be specified.");

            Type actualEnumType = Nullable.GetUnderlyingType(this._enumType) ?? this._enumType;
            Array enumValues = Enum.GetValues(actualEnumType);

            if (actualEnumType == this._enumType)
                return enumValues;

            Array tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
            enumValues.CopyTo(tempArray, 1);
            return tempArray;
        }
    }
}
