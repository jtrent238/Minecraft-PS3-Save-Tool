using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace NBTSharp
{
    public class NBTFileWriter
    {
        public static void write(String path, TagCompound tag)
        {
            FileStream stream = null;
            //GZipStream gzip = null;
            BinaryWriter writer = null;

            try
            {
                stream = new FileStream(path, FileMode.Create, FileAccess.Write);
               // gzip = new GZipStream(stream, CompressionMode.Compress);
                writer = new BinaryWriter(stream);
                tag.write(writer);
            }
            catch (IOException exception)
            {
                throw new FailedWriteException("An IOException occurred while writing NBT file", exception);
            }
            catch (Exception exception)
            {
                throw new FailedWriteException("An exception occurred while writing NBT file", exception);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
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
