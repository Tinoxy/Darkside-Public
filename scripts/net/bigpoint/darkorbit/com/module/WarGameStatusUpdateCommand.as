package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class WarGameStatusUpdateCommand implements IModule
   {
      private static var _instance:WarGameStatusUpdateCommand;
      
      public static const TEAM_RED:int = 0;
      
      public static const TEAM_BLUE:int = 1;
      
      public static const TEAM__MAX:int = 2;
      
      public var remainingSeconds:int = 0;
      
      public var invasionTimeLeft:int = 0;
      
      public var waveCount:int = 0;
      
      public var isInvading:Boolean = false;
      
      public var team:int = 0;
      
      public var remainingLifes:int = 0;
      
      public function WarGameStatusUpdateCommand(param1:int = 0, param2:int = 0, param3:int = 0, param4:int = 0, param5:int = 0, param6:Boolean = false)
      {
         super();
         this.remainingSeconds = param3;
         this.invasionTimeLeft = param5;
         this.waveCount = param4;
         this.isInvading = param6;
         this.team = param1;
         this.remainingLifes = param2;
      }
      
      public static function get instance() : WarGameStatusUpdateCommand
      {
         return _instance || (_instance = new WarGameStatusUpdateCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -11262;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 17;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.remainingSeconds = param1.readInt();
         this.remainingSeconds = this.remainingSeconds >>> 3 | this.remainingSeconds << 29;
         this.invasionTimeLeft = param1.readInt();
         this.invasionTimeLeft = this.invasionTimeLeft << 3 | this.invasionTimeLeft >>> 29;
         this.waveCount = param1.readInt();
         this.waveCount = this.waveCount << 9 | this.waveCount >>> 23;
         this.isInvading = param1.readBoolean();
         this.team = param1.readShort();
         this.remainingLifes = param1.readInt();
         this.remainingLifes = this.remainingLifes << 10 | this.remainingLifes >>> 22;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-11262);
         param1.writeInt(this.remainingSeconds << 3 | this.remainingSeconds >>> 29);
         param1.writeInt(this.invasionTimeLeft >>> 3 | this.invasionTimeLeft << 29);
         param1.writeInt(this.waveCount >>> 9 | this.waveCount << 23);
         param1.writeBoolean(this.isInvading);
         param1.writeShort(this.team);
         param1.writeInt(this.remainingLifes >>> 10 | this.remainingLifes << 22);
      }
   }
}

