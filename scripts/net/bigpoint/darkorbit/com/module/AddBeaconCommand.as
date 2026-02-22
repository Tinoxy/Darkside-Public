package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   
   public class AddBeaconCommand extends AddMapEntityCommand
   {
      private static var _instance:AddBeaconCommand;
      
      public var beaconId:int = 0;
      
      public function AddBeaconCommand(param1:String = "", param2:int = 0, param3:int = 0, param4:int = 0)
      {
         super(param1,param2,param3);
         this.beaconId = param4;
      }
      
      public static function get instance() : AddBeaconCommand
      {
         return _instance || (_instance = new AddBeaconCommand());
      }
      
      override public function getLibcomModuleId() : int
      {
         return 10599;
      }
      
      override public function getLibcomModuleEstimatedLength() : int
      {
         return 4;
      }
      
      override public function read(param1:IDataInput) : void
      {
         param1.readShort();
         super.read(param1);
         this.beaconId = param1.readInt();
         this.beaconId = this.beaconId << 9 | this.beaconId >>> 23;
      }
      
      override public function write(param1:IDataOutput) : void
      {
         param1.writeShort(10599);
         super.write(param1);
         param1.writeInt(this.beaconId >>> 9 | this.beaconId << 23);
      }
   }
}

