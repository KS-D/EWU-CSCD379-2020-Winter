using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Data.Tests
{
    static public class SampleData
    {
        

        public  const string RingTitle = "Ring Doorbell";
        public const string RingUrl = "www.ring.com";
        public const string RingDescription = "The doorbell that saw too much";

        static public Gift CreateRingGift() => new Gift(RingTitle,RingDescription, RingUrl, CreateInigo());
        
        public const string ArduinoTitle = "Arduino";
        public const string ArduinoUrl = "www.arduino.com";
        public const string ArduinoDescription = "Every good geek needs an IOT device";

        public static Gift CreateGiftArduino() =>
            new Gift(ArduinoTitle, ArduinoDescription, ArduinoUrl, CreateInigo());

        public const string _Inigo = "Inigo";
        public const string _Montoya = "Montoya";
       
        static public User CreateInigo() => new User(_Inigo, _Montoya);
 
    }
}
