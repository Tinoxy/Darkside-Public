package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class EternalBlacklightWaveInfoCommand implements IModule
   {
      private static var _instance:EternalBlacklightWaveInfoCommand;
      
      public var currentWave:int = 0;
      
      public var furthestWave:int = 0;
      
      public function EternalBlacklightWaveInfoCommand(param1:int = 0, param2:int = 0)
      {
         super();
         this.currentWave = param1;
         this.furthestWave = param2;
      }
      
      public static function get instance() : EternalBlacklightWaveInfoCommand
      {
         return _instance || (_instance = new EternalBlacklightWaveInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 4441;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 8;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.currentWave = param1.readInt();
         this.currentWave = this.currentWave << 3 | this.currentWave >>> 29;
         this.furthestWave = param1.readInt();
         this.furthestWave = this.furthestWave << 2 | this.furthestWave >>> 30;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(4441);
         param1.writeInt(this.currentWave >>> 3 | this.currentWave << 29);
         param1.writeInt(this.furthestWave >>> 2 | this.furthestWave << 30);
      }
   }
}

