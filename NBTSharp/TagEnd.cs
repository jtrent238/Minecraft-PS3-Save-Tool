using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NBTSharp
{
    public class TagEnd : Tag
    {
        public const byte TagType = 0;

        public TagEnd()
        {
            this.tagType = 0;
            this.name = "";
        }

        public override void read(BinaryReader reader)
        {
        }

        public override void writeData(BinaryWriter writer)
        {
        }

        public static bool isValidString(string text)
        {
            return true;
        }
    }
}
