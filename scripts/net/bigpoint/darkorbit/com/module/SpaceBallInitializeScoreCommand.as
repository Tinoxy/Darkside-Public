package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class SpaceBallInitializeScoreCommand implements IModule
   {
      private static var _instance:SpaceBallInitializeScoreCommand;
      
      public var scoreMars:int = 0;
      
      public var scoreEarth:int = 0;
      
      public var scoreVenus:int = 0;
      
      public var ownerFactionId:int = 0;
      
      public var speed:int = 0;
      
      public function SpaceBallInitializeScoreCommand(param1:int = 0, param2:int = 0, param3:int = 0, param4:int = 0, param5:int = 0)
      {
         super();
         this.scoreMars = param1;
         this.scoreEarth = param2;
         this.scoreVenus = param3;
         this.ownerFactionId = param4;
         this.speed = param5;
      }
      
      public static function get instance() : SpaceBallInitializeScoreCommand
      {
         return _instance || (_instance = new SpaceBallInitializeScoreCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 190;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 20;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.scoreMars = param1.readInt();
         this.scoreMars = this.scoreMars >>> 2 | this.scoreMars << 30;
         this.scoreEarth = param1.readInt();
         this.scoreEarth = this.scoreEarth << 5 | this.scoreEarth >>> 27;
         this.scoreVenus = param1.readInt();
         this.scoreVenus = this.scoreVenus >>> 8 | this.scoreVenus << 24;
         this.ownerFactionId = param1.readInt();
         this.ownerFactionId = this.ownerFactionId >>> 12 | this.ownerFactionId << 20;
         this.speed = param1.readInt();
         this.speed = this.speed << 3 | this.speed >>> 29;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(190);
         param1.writeInt(this.scoreMars << 2 | this.scoreMars >>> 30);
         param1.writeInt(this.scoreEarth >>> 5 | this.scoreEarth << 27);
         param1.writeInt(this.scoreVenus << 8 | this.scoreVenus >>> 24);
         param1.writeInt(this.ownerFactionId << 12 | this.ownerFactionId >>> 20);
         param1.writeInt(this.speed >>> 3 | this.speed << 29);
      }
   }
}

