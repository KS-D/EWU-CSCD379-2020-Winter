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

        static public Gift CreateRingGift() => new Gift(RingTitle,RingDescription, RingUrl, CreateUserInigo());
        
        public const string ArduinoTitle = "Arduino";
        public const string ArduinoUrl = "www.arduino.com";
        public const string ArduinoDescription = "Every good geek needs an IOT device";

        public static Gift CreateGiftArduino() =>
            new Gift(ArduinoTitle, ArduinoDescription, ArduinoUrl, CreateUserButtercup());

        public const string Inigo = "Inigo";
        public const string Montoya = "Montoya";
       
        static public User CreateUserInigo() => new User(Inigo, Montoya);

        public const string Princess = "Princess";
        public const string Buttercup = "Buttercup";

        static public User CreateUserButtercup() => new User(Princess, Buttercup);

        public const string ForestGroupTitle = "Enchanted Forest";

        public static Group CreateEnchantedForestGroup() => new Group(ForestGroupTitle);

        public const string CastleGroupTitle = "Its a castle!";
        
        public static Group CreateCastleGroup() => new Group(CastleGroupTitle);
    }
}
