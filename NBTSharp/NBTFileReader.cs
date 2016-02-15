using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace NBTSharp
{
    public class NBTFileReader
    {
        public String path;
        public TagCompound compound;

        public NBTFileReader(String path)
        {
            this.path = path;

            FileStream stream = null;
           // GZipStream gzip = null;
            BinaryReader reader = null;

            try
            {

                stream = new FileStream(this.path, FileMode.Open, FileAccess.Read);
               // gzip = new GZipStream(stream, CompressionMode.Decompress);
                reader = new BinaryReader(stream);

                //file must start with a tag compound, whose tag type is 10
                byte tagType = reader.ReadByte();

                if (tagType != TagCompound.TagType)
                {
                    throw new InvalidTagException("Tag type at the beginning of the file must be 10");
                }
                else
                {
                    compound = new TagCompound();
                    compound.read(reader);
                }
            }
            catch (InvalidTagException)
            {
                throw;
            }
            catch (IOException exception)
            {
                throw new FailedReadException(exception.Message, exception);
            }
            catch (Exception exception)
            {
                throw new FailedReadException(exception.Message, exception);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

              /*  if (gzip != null)
                {
                    gzip.Close();
                }*/

                if (stream != null)
                {
                    stream.Close();
                }
            }
        }
    }
}
