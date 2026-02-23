package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class TitleFrenzyRankingInfoModule implements IModule
   {
      private static var _instance:TitleFrenzyRankingInfoModule;
      
      public var rank:int = 0;
      
      public var username:String = "";
      
      public var score:Number = 0;
      
      public function TitleFrenzyRankingInfoModule(param1:int = 0, param2:String = "", param3:Number = 0)
      {
         super();
         this.rank = param1;
         this.username = param2;
         this.score = param3;
      }
      
      public static function get instance() : TitleFrenzyRankingInfoModule
      {
         return _instance || (_instance = new TitleFrenzyRankingInfoModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return -26803;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 10;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.rank = param1.readInt();
         this.rank = this.rank >>> 3 | this.rank << 29;
         this.username = param1.readUTF();
         this.score = param1.readFloat();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-26803);
         param1.writeInt(this.rank << 3 | this.rank >>> 29);
         param1.writeUTF(this.username);
         param1.writeFloat(this.score);
      }
   }
}

