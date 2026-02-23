package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class GalaxyGateBuilderGateCompleteCommand implements IModule
   {
      private static var _instance:GalaxyGateBuilderGateCompleteCommand;
      
      public var currPart:int = 0;
      
      public var gateId:int = 0;
      
      public function GalaxyGateBuilderGateCompleteCommand(param1:int = 0, param2:int = 0)
      {
         super();
         this.currPart = param2;
         this.gateId = param1;
      }
      
      public static function get instance() : GalaxyGateBuilderGateCompleteCommand
      {
         return _instance || (_instance = new GalaxyGateBuilderGateCompleteCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 2414;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 3;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.currPart = param1.readShort();
         this.currPart = 65535 & ((65535 & this.currPart) >>> 0 | (65535 & this.currPart) << 16);
         this.currPart = this.currPart > 32767 ? int(this.currPart - 65536) : this.currPart;
         this.gateId = param1.readByte();
         this.gateId = 255 & ((255 & this.gateId) << 1 | (255 & this.gateId) >>> 7);
         this.gateId = this.gateId > 127 ? int(this.gateId - 256) : this.gateId;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(2414);
         param1.writeShort(65535 & ((65535 & this.currPart) << 0 | (65535 & this.currPart) >>> 16));
         param1.writeByte(255 & ((255 & this.gateId) >>> 1 | (255 & this.gateId) << 7));
      }
   }
}

