using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NBTSharp
{
    public abstract class Tag
    {
        public byte tagType;
        public String name;
        public abstract void read(BinaryReader reader);
        public abstract void writeData(BinaryWriter writer);
        
        public Tag getNode(String path)
        {
            String[] delim = { "\\" };
            String[] tokens = path.Split(delim, StringSplitOptions.None);

            if (tokens.Length == 0)
            {
                throw new InvalidPathException("Node path is empty");
            }
            else if (tokens.Length == 1 && tokens[0] == this.name)
            {
                return this;
            }
            else
            {
                //find the node
                String nodeName = tokens[1];
                Tag tag = this.findTagByName(nodeName);

                if (tag != null)
                {
                    if (tokens.Length > 2)
                    {
                        //traverse the node
                        path = path.Substring(path.IndexOf("\\") + 1);

                        /**/
                        //Console.WriteLine("Found node");
                        //Console.WriteLine("Traversing to: " + path);

                        byte tagType = tag.tagType;

                        //check if the tag can be traversed
                        if (tag.isSimpleTag())
                        {
                            throw new InvalidPathException(Util.getTypeName(tagType) +
                                " can't be traversed");
                        }
                        else
                        {
                            return tag.getNode(path);
                        }
                    }
                    else if (tokens.Length == 2)
                    {
                        return tag;
                    }
                }
                else
                {
                    throw new InvalidPathException(path + " not found");
                }
            }

            return null;
        }

        public virtual Tag findTagByName(String name)
        {
            return null;
        }

        public void write(BinaryWriter writer)
        {
            try
            {
                //writer type and name
                writer.Write(this.tagType);
                Util.writeString(writer, this.name);

                //write any additional data, which will be defined in the appropriate class
                writeData(writer);
            }
            catch (IOException exception)
            {
                throw new FailedWriteException(exception.Message);
            }
        }

        //compound, byte array and list contain multiple tags or data
        //all the rest only contain one piece of data, and are referred to as simple tags in this program
        public bool isSimpleTag()
        {
            return (this.tagType != TagCompound.TagType &&
                    this.tagType != TagList.TagType &&
                    this.tagType != TagByteArray.TagType);
        }
    }
}
