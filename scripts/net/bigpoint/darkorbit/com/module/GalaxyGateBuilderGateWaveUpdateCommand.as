package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class GalaxyGateBuilderGateWaveUpdateCommand implements IModule
   {
      private static var _instance:GalaxyGateBuilderGateWaveUpdateCommand;
      
      public var gateId:int = 0;
      
      public var currWave:int = 0;
      
      public function GalaxyGateBuilderGateWaveUpdateCommand(param1:int = 0, param2:int = 0)
      {
         super();
         this.gateId = param1;
         this.currWave = param2;
      }
      
      public static function get instance() : GalaxyGateBuilderGateWaveUpdateCommand
      {
         return _instance || (_instance = new GalaxyGateBuilderGateWaveUpdateCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 8778;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 3;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.gateId = param1.readByte();
         this.gateId = 255 & ((255 & this.gateId) >>> 1 | (255 & this.gateId) << 7);
         this.gateId = this.gateId > 127 ? int(this.gateId - 256) : this.gateId;
         this.currWave = param1.readShort();
         this.currWave = 65535 & ((65535 & this.currWave) >>> 14 | (65535 & this.currWave) << 2);
         this.currWave = this.currWave > 32767 ? int(this.currWave - 65536) : this.currWave;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(8778);
         param1.writeByte(255 & ((255 & this.gateId) << 1 | (255 & this.gateId) >>> 7));
         param1.writeShort(65535 & ((65535 & this.currWave) << 14 | (65535 & this.currWave) >>> 2));
      }
   }
}

