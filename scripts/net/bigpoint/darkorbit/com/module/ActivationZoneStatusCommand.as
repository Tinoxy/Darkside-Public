package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class ActivationZoneStatusCommand implements IModule
   {
      private static var _instance:ActivationZoneStatusCommand;
      
      public var currentValue:int = 0;
      
      public var maxValue:int = 0;
      
      public var poiId:String = "";
      
      public function ActivationZoneStatusCommand(param1:String = "", param2:int = 0, param3:int = 0)
      {
         super();
         this.currentValue = param2;
         this.maxValue = param3;
         this.poiId = param1;
      }
      
      public static function get instance() : ActivationZoneStatusCommand
      {
         return _instance || (_instance = new ActivationZoneStatusCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 25758;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 10;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.currentValue = param1.readInt();
         this.currentValue = this.currentValue >>> 13 | this.currentValue << 19;
         this.maxValue = param1.readInt();
         this.maxValue = this.maxValue >>> 8 | this.maxValue << 24;
         this.poiId = param1.readUTF();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(25758);
         param1.writeInt(this.currentValue << 13 | this.currentValue >>> 19);
         param1.writeInt(this.maxValue << 8 | this.maxValue >>> 24);
         param1.writeUTF(this.poiId);
      }
   }
}

