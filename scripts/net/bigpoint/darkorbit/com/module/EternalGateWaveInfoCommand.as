package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class EternalGateWaveInfoCommand implements IModule
   {
      private static var _instance:EternalGateWaveInfoCommand;
      
      public var nextMilestone:int = 0;
      
      public var furthestWave:int = 0;
      
      public var currentWave:int = 0;
      
      public function EternalGateWaveInfoCommand(param1:int = 0, param2:int = 0, param3:int = 0)
      {
         super();
         this.nextMilestone = param3;
         this.furthestWave = param2;
         this.currentWave = param1;
      }
      
      public static function get instance() : EternalGateWaveInfoCommand
      {
         return _instance || (_instance = new EternalGateWaveInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -3035;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 12;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.nextMilestone = param1.readInt();
         this.nextMilestone = this.nextMilestone >>> 15 | this.nextMilestone << 17;
         this.furthestWave = param1.readInt();
         this.furthestWave = this.furthestWave << 9 | this.furthestWave >>> 23;
         this.currentWave = param1.readInt();
         this.currentWave = this.currentWave << 12 | this.currentWave >>> 20;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-3035);
         param1.writeInt(this.nextMilestone << 15 | this.nextMilestone >>> 17);
         param1.writeInt(this.furthestWave >>> 9 | this.furthestWave << 23);
         param1.writeInt(this.currentWave >>> 12 | this.currentWave << 20);
      }
   }
}

