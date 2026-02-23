package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class KillUiInfoMessageCommand implements IModule
   {
      private static var _instance:KillUiInfoMessageCommand;
      
      public var mapId:int = 0;
      
      public var spawnNpcId:int = 0;
      
      public var currentNpcKills:int = 0;
      
      public var killNpcId:int = 0;
      
      public var maxNpcAmountForMap:int = 0;
      
      public var currentNpcOnMap:int = 0;
      
      public function KillUiInfoMessageCommand(param1:int = 0, param2:int = 0, param3:int = 0, param4:int = 0, param5:int = 0, param6:int = 0)
      {
         super();
         this.mapId = param1;
         this.spawnNpcId = param3;
         this.currentNpcKills = param6;
         this.killNpcId = param2;
         this.maxNpcAmountForMap = param5;
         this.currentNpcOnMap = param4;
      }
      
      public static function get instance() : KillUiInfoMessageCommand
      {
         return _instance || (_instance = new KillUiInfoMessageCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -9335;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 24;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.mapId = param1.readInt();
         this.mapId = this.mapId << 14 | this.mapId >>> 18;
         this.spawnNpcId = param1.readInt();
         this.spawnNpcId = this.spawnNpcId << 13 | this.spawnNpcId >>> 19;
         this.currentNpcKills = param1.readInt();
         this.currentNpcKills = this.currentNpcKills >>> 11 | this.currentNpcKills << 21;
         this.killNpcId = param1.readInt();
         this.killNpcId = this.killNpcId >>> 13 | this.killNpcId << 19;
         this.maxNpcAmountForMap = param1.readInt();
         this.maxNpcAmountForMap = this.maxNpcAmountForMap << 4 | this.maxNpcAmountForMap >>> 28;
         this.currentNpcOnMap = param1.readInt();
         this.currentNpcOnMap = this.currentNpcOnMap << 1 | this.currentNpcOnMap >>> 31;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-9335);
         param1.writeInt(this.mapId >>> 14 | this.mapId << 18);
         param1.writeInt(this.spawnNpcId >>> 13 | this.spawnNpcId << 19);
         param1.writeInt(this.currentNpcKills << 11 | this.currentNpcKills >>> 21);
         param1.writeInt(this.killNpcId << 13 | this.killNpcId >>> 19);
         param1.writeInt(this.maxNpcAmountForMap >>> 4 | this.maxNpcAmountForMap << 28);
         param1.writeInt(this.currentNpcOnMap >>> 1 | this.currentNpcOnMap << 31);
      }
   }
}

