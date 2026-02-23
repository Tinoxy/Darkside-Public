package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class SpaceBallUpdateScoreCommand implements IModule
   {
      private static var _instance:SpaceBallUpdateScoreCommand;
      
      public var factionId:int = 0;
      
      public var score:int = 0;
      
      public var gateId:int = 0;
      
      public function SpaceBallUpdateScoreCommand(param1:int = 0, param2:int = 0, param3:int = 0)
      {
         super();
         this.factionId = param1;
         this.score = param2;
         this.gateId = param3;
      }
      
      public static function get instance() : SpaceBallUpdateScoreCommand
      {
         return _instance || (_instance = new SpaceBallUpdateScoreCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 191;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 12;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.factionId = param1.readInt();
         this.factionId = this.factionId << 1 | this.factionId >>> 31;
         this.score = param1.readInt();
         this.score = this.score >>> 10 | this.score << 22;
         this.gateId = param1.readInt();
         this.gateId = this.gateId << 9 | this.gateId >>> 23;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(191);
         param1.writeInt(this.factionId >>> 1 | this.factionId << 31);
         param1.writeInt(this.score << 10 | this.score >>> 22);
         param1.writeInt(this.gateId >>> 9 | this.gateId << 23);
      }
   }
}

