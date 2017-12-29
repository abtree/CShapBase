using System.IO;
using System;

namespace UserException
{
    public class SalesSpyFoundException : Exception
    {
        public SalesSpyFoundException(string name)
            : base(name)
        {

        }

        public SalesSpyFoundException(string name, Exception innerEx)
            :base(name, innerEx)
        {

        }
    }

    public class ColdCallFileFormatException : Exception
    {
        public ColdCallFileFormatException(string name)
            : base(name)
        {

        }

        public ColdCallFileFormatException(string name, Exception innerEx)
            :base(name, innerEx)
        {

        }
    }

    public class UnexpectedException : Exception
    {
        public UnexpectedException(string name)
            : base(name)
        {

        }

        public UnexpectedException(string name, Exception innerEx)
            :base(name, innerEx)
        {

        }
    }
    public class ColdCallFileReader : IDisposable
    {
        private FileStream _fs;
        private StreamReader _sr;
        private uint _nPeopleToRing;
        private bool _isDisposed = false;
        private bool _isOpen = false;

        public void Open(string filename)
        {
            if (_isDisposed)
                throw new ObjectDisposedException("peopleToRing");

            _fs = new FileStream(filename, FileMode.Open);
            _sr = new StreamReader(_fs);

            try
            {
                string firstLine = _sr.ReadLine();
                _nPeopleToRing = uint.Parse(firstLine);
                _isOpen = true;
            }catch(FormatException ex)
            {
                throw new ColdCallFileFormatException(@"First line isn't an integer", ex);
            }
        }

        public void ProcessNextPerson()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("peopleToRing");
            }

            if (!_isOpen)
            {
                throw new UnexpectedException("Attempted to access cold call file that is not open");
            }

            try
            {
                string name = _sr.ReadLine();
                if(name == null)
                {
                    throw new ColdCallFileFormatException("not enough names");
                }
                if(name[0] == 'B')
                {
                    throw new SalesSpyFoundException(name);
                }
                Console.WriteLine(name);
            }catch(SalesSpyFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

            }
        }

        public uint NPeopleToRing
        {
            get
            {
                if (_isDisposed)
                    throw new ObjectDisposedException("peopleToRing");

                if (!_isOpen)
                    throw new UnexpectedException("Attempted to access cold call file that is not open");

                return _nPeopleToRing;
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;
            _isDisposed = true;
            _isOpen = false;
            if(_fs != null)
            {
                _fs.Close();
                _fs = null;
            }
        }
    }
    class UserException
    {

    }
}
