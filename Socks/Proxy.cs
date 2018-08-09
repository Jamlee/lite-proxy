// https://samsclass.info/122/proj/how-socks5-works.html

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Serilog;
using System.IO;

namespace LiteSocks
{
    class SocksVersionException : Exception
    {
        public SocksVersionException(String message) : base(message)
        {
        }
    }

    class SocksDomainException : Exception
    {
        public SocksDomainException(String message) : base(message)
        {
        }
    }

    class SocksUpsteamPortException : Exception
    {
        public SocksUpsteamPortException(String message) : base(message)
        {
        }
    }

    public class Proxy
    {
        private TcpListener tcpListener;

        public Proxy(String bindIp, Int32 port)
        {
            IPAddress localAddr = IPAddress.Parse(bindIp);
            tcpListener = new TcpListener(localAddr, port);
        }

        public Int32 ParseSocksVersion(NetworkStream stream)
        {
            Int32 numBytesToRead = 3;
            Int32 numberOfByteshasRead = 0;
            var bytes = new Byte[3];
            do
            {
                Int32 n = stream.Read(bytes, numberOfByteshasRead, numBytesToRead);
                numberOfByteshasRead += n;
                numBytesToRead -= n;
            } while (numBytesToRead > 0);

            if (bytes[0] != 5)
                throw new SocksVersionException("socks version is wrong");

            stream.Write(new Byte[] { 0x5, 0x0 }, 0, 2);
            return bytes[0];
        }

        public String GetHost(NetworkStream stream, Byte flag)
        {
            var bytes = new Byte[1024];

            switch (flag)
            {
                // 如果采用的是域名
                case 0x03:
                    // 获取域名长度
                    var numBytesToRead = 1;
                    var numberOfByteshasRead = 0;
                    Array.Clear(bytes, 0, bytes.Length);
                    do
                    {
                        Int32 n = stream.Read(bytes, numberOfByteshasRead, numBytesToRead);
                        numBytesToRead -= n;
                        numberOfByteshasRead += n;
                    } while (numBytesToRead > 0);

                    // 获取域名
                    numBytesToRead = bytes[0];
                    numberOfByteshasRead = 0;
                    Array.Clear(bytes, 0, bytes.Length);
                    do
                    {
                        Int32 n = stream.Read(bytes, numberOfByteshasRead, numBytesToRead);
                        numBytesToRead -= n;
                        numberOfByteshasRead += n;
                    } while (numBytesToRead > 0);

                    Log.Logger.Information("Socks5: upstream domian is {0}",
                        Encoding.ASCII.GetString(bytes, 0, numberOfByteshasRead));
                    var domain = Encoding.ASCII.GetString(bytes, 0, numberOfByteshasRead);
                    return domain;

                case 0x01:
                    break;

                default:
                    throw new SocksDomainException("can not get upstream domain or ip");
            }

            return null;
        }

        public Int32 GetPort(NetworkStream stream)
        {
            Int32 numBytesToRead = 2;
            Int32 numberOfByteshasRead = 0;
            var bytes = new Byte[2];
            Array.Clear(bytes, 0, bytes.Length);
            do
            {
                Int32 n = stream.Read(bytes, numberOfByteshasRead, numBytesToRead);
                numberOfByteshasRead += n;
                numBytesToRead -= n;
            } while (numBytesToRead > 0);
            if (numberOfByteshasRead == 0)
                throw new SocksUpsteamPortException("can not parse upstream port");
            Log.Logger.Information("Socks5: upstream port {0}", bytes[0] * 256 + bytes[1]);
            return bytes[0] * 256 + bytes[1];
        }

        public void ResponseToSocks(NetworkStream stream)
        {
            var sockResponse = new Byte[] {
                0x05, 0x00, 0x00, 0x01,
                0x00, 0x00, 0x00, 0x00,
                0x1f, 0x40
            };
            stream.Write(sockResponse, 0, sockResponse.Length);
        }

        public async void Run(NetworkStream stream)
        {
            try
            {
                // 解析 socks 头
                Int32 numBytesToRead = 4;
                Int32 numberOfByteshasRead = 0;
                var bytes = new Byte[4];
                var version = ParseSocksVersion(stream);
                do
                {
                    Int32 n = stream.Read(bytes, numberOfByteshasRead, numBytesToRead);
                    numberOfByteshasRead += n;
                    numBytesToRead -= n;
                } while (numBytesToRead > 0);

                var conn = new Connection(GetHost(stream, bytes[3]), GetPort(stream), stream);
                ResponseToSocks(stream);

                // 转发请求并获取响应
                await conn.Response();
            }
            catch (IOException e)
            {
                Log.Logger.Information(e.Message);
            }
            catch (SocksVersionException e)
            {
                Log.Logger.Information(e.Message);
            }
            catch (SocksDomainException e)
            {
                Log.Logger.Information(e.Message);
            }
            catch (SocksUpsteamPortException e)
            {
                Log.Logger.Information(e.Message);
            }
            catch (Exception e)
            {
                Log.Logger.Information(e.Message);
            }
        }

        public void Start()
        {
            tcpListener.Start();

            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
                Run(stream);
            }
        }
    }
}
