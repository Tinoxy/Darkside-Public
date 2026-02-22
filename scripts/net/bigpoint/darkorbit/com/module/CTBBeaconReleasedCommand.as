package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class CTBBeaconReleasedCommand implements IModule
   {
      private static var _instance:CTBBeaconReleasedCommand;
      
      public var userId:int = 0;
      
      public var beaconId:int = 0;
      
      public function CTBBeaconReleasedCommand(param1:int = 0, param2:int = 0)
      {
         super();
         this.userId = param1;
         this.beaconId = param2;
      }
      
      public static function get instance() : CTBBeaconReleasedCommand
      {
         return _instance || (_instance = new CTBBeaconReleasedCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 221;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 8;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.userId = param1.readInt();
         this.userId = this.userId >>> 11 | this.userId << 21;
         this.beaconId = param1.readInt();
         this.beaconId = this.beaconId >>> 3 | this.beaconId << 29;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(221);
         param1.writeInt(this.userId << 11 | this.userId >>> 21);
         param1.writeInt(this.beaconId << 3 | this.beaconId >>> 29);
      }
   }
}

