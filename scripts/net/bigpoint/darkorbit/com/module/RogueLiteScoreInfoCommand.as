package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class RogueLiteScoreInfoCommand implements IModule
   {
      private static var _instance:RogueLiteScoreInfoCommand;
      
      public var score:int = 0;
      
      public var progress:int = 0;
      
      public function RogueLiteScoreInfoCommand(param1:int = 0, param2:int = 0)
      {
         super();
         this.score = param2;
         this.progress = param1;
      }
      
      public static function get instance() : RogueLiteScoreInfoCommand
      {
         return _instance || (_instance = new RogueLiteScoreInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 18403;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 8;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.score = param1.readInt();
         this.score = this.score >>> 15 | this.score << 17;
         this.progress = param1.readInt();
         this.progress = this.progress << 3 | this.progress >>> 29;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(18403);
         param1.writeInt(this.score << 15 | this.score >>> 17);
         param1.writeInt(this.progress >>> 3 | this.progress << 29);
      }
   }
}

