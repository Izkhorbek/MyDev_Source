using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Dynamic;
using System.IO.BACnet;
using System.IO.BACnet.Serialize;
using System.Linq;
using System.Diagnostics;
using LibCommonDef;
namespace Subway_BACnet
{
    public class ReadDatas
    {
        public string[] ReadBACnetData(int device_id, int pointIns)
        {
            string[] _retArray = new string[2];
            _retArray[0] = string.Empty;
            _retArray[1] = string.Empty;

            BacnetValue _presentValue, _objectName;
            bool retName, retValue;

            // Read Present_Value property on the object ANALOG_INPUT:0 provided by the device 12345
            // Scalar value only
            retName = ReadScalarValue(device_id, new BacnetObjectId(BacnetObjectTypes.OBJECT_BINARY_INPUT, (uint)pointIns), BacnetPropertyIds.PROP_OBJECT_NAME, out _objectName);
            if (retName == true)
            {
                _retArray[0] = _objectName.Value == null ? "-1" : _objectName.Value.ToString();
            }
            else
            {
                _retArray[0] = "-1";
            }

            retValue = ReadScalarValue(device_id, new BacnetObjectId(BacnetObjectTypes.OBJECT_BINARY_INPUT, (uint)pointIns), BacnetPropertyIds.PROP_PRESENT_VALUE, out _presentValue);
            if (retValue == true)
            {
                if(_presentValue.Value == null || _presentValue.Value.ToString() == "-1")
                {
                    _retArray[1] = "255";
                }
                else
                {
                    _retArray[1] = _presentValue.Value.ToString();
                }
            }
            else
            {
                _retArray[1] = "255";
            }

            return _retArray;
        }

        public bool ReadScalarValue(int device_id, BacnetObjectId BacnetObjet, BacnetPropertyIds Propriete, out BacnetValue Value)
        {
            BacnetAddress adr;
            IList<BacnetValue> NoScalarValue = null;

            Value = new BacnetValue(null);

            // Looking for the device
            adr = DeviceAddr((uint)device_id);
            if (adr == null) return false;  // not found
            try
            {
                if (!Processing.Instance.bacnet_client.ReadPropertyRequest(adr, BacnetObjet, Propriete, out NoScalarValue))
                {
                    Console.WriteLine("Couldn't fetch objects Communication Error");
                    return false;
                }

                if (NoScalarValue != null)
                {
                    Value = NoScalarValue[0];
                }
            }
            catch
            {
                Value.Value = "-1";
            }
            return true;
        }

        public BacnetAddress DeviceAddr(uint device_id)
        {
            BacnetAddress ret;

            lock (Processing._instance.DevicesList)
            {
                foreach (BacNode bn in Processing._instance.DevicesList)
                {
                    ret = bn.getAdd(device_id);
                    if (ret != null) return ret;
                }
                // not in the list
                return null;
            }
        }
    }
}
