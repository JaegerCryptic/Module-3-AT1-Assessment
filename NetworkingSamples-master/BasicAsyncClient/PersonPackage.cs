////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	personpackage.cs
//
// summary:	Implements the personpackage class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;

namespace BasicAsyncClient
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A person package. </summary>
    ///
    /// <remarks>   David Hunt, 8/19/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    class PersonPackage
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets a value indicating whether this object is male. </summary>
        ///
        /// <value> True if this object is male, false if not. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool IsMale { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the age. </summary>
        ///
        /// <value> The age. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public ushort Age { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public string Name { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deserialize data received. </summary>
        ///
        /// <remarks>   David Hunt, 8/19/2018. </remarks>
        ///
        /// <param name="male"> True to male. </param>
        /// <param name="age">  The age. </param>
        /// <param name="name"> The name. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public PersonPackage(bool male, ushort age, string name)
        {
            IsMale = male;
            Age = age;
            Name = name;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deserialize data received. </summary>
        ///
        /// <remarks>   David Hunt, 8/19/2018. </remarks>
        ///
        /// <param name="data"> The data. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public PersonPackage(byte[] data)
        {
            IsMale = BitConverter.ToBoolean(data, 0);
            Age = BitConverter.ToUInt16(data, 1);
            int nameLength = BitConverter.ToInt32(data, 3);
            Name = Encoding.ASCII.GetString(data, 7, nameLength);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Serializes this package to a byte array. </summary>
        ///
        /// <remarks>
        /// Use the <see cref="Buffer"/> or <see cref="Array"/> class for better performance.
        /// </remarks>
        ///
        /// <returns>   This object as a byte[]. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public byte[] ToByteArray()
        {
            List<byte> byteList = new List<byte>();
            byteList.AddRange(BitConverter.GetBytes(IsMale));
            byteList.AddRange(BitConverter.GetBytes(Age));
            byteList.AddRange(BitConverter.GetBytes(Name.Length));
            byteList.AddRange(Encoding.ASCII.GetBytes(Name));
            return byteList.ToArray();
        }
    }
}
