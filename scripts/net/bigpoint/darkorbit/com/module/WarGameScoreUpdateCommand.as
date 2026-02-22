package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class WarGameScoreUpdateCommand implements IModule
   {
      private static var _instance:WarGameScoreUpdateCommand;
      
      public var teamRedScore:int = 0;
      
      public var score:int = 0;
      
      public var teamBlueScore:int = 0;
      
      public function WarGameScoreUpdateCommand(param1:int = 0, param2:int = 0, param3:int = 0)
      {
         super();
         this.teamRedScore = param1;
         this.score = param3;
         this.teamBlueScore = param2;
      }
      
      public static function get instance() : WarGameScoreUpdateCommand
      {
         return _instance || (_instance = new WarGameScoreUpdateCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -25182;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 12;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.teamRedScore = param1.readInt();
         this.teamRedScore = this.teamRedScore << 12 | this.teamRedScore >>> 20;
         this.score = param1.readInt();
         this.score = this.score >>> 7 | this.score << 25;
         this.teamBlueScore = param1.readInt();
         this.teamBlueScore = this.teamBlueScore << 16 | this.teamBlueScore >>> 16;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-25182);
         param1.writeInt(this.teamRedScore >>> 12 | this.teamRedScore << 20);
         param1.writeInt(this.score << 7 | this.score >>> 25);
         param1.writeInt(this.teamBlueScore >>> 16 | this.teamBlueScore << 16);
      }
   }
}

