package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class WarGameLobbyStatusModule implements IModule
   {
      private static var _instance:WarGameLobbyStatusModule;
      
      public static const STATUS_GEAR_SCORE_TOO_LOW:int = 0;
      
      public static const STATUS_READY:int = 1;
      
      public static const STATUS_IN_QUEUE:int = 2;
      
      public static const STATUS__MAX:int = 3;
      
      public var minPlayers:int = 0;
      
      public var playerCount:int = 0;
      
      public var status:int = 0;
      
      public function WarGameLobbyStatusModule(param1:int = 0, param2:int = 0, param3:int = 0)
      {
         super();
         this.minPlayers = param3;
         this.playerCount = param2;
         this.status = param1;
      }
      
      public static function get instance() : WarGameLobbyStatusModule
      {
         return _instance || (_instance = new WarGameLobbyStatusModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return -8974;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 8;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.minPlayers = param1.readInt();
         this.minPlayers = this.minPlayers >>> 7 | this.minPlayers << 25;
         this.playerCount = param1.readInt();
         this.playerCount = this.playerCount << 5 | this.playerCount >>> 27;
         this.status = param1.readShort();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-8974);
         param1.writeInt(this.minPlayers << 7 | this.minPlayers >>> 25);
         param1.writeInt(this.playerCount >>> 5 | this.playerCount << 27);
         param1.writeShort(this.status);
      }
   }
}

