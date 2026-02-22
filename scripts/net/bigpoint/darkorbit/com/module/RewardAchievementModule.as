package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   
   public class RewardAchievementModule extends RewardContentModule
   {
      private static var _instance:RewardAchievementModule;
      
      public var level:int = 0;
      
      public var achievementKey:String = "";
      
      public function RewardAchievementModule(param1:String = "", param2:int = 0)
      {
         super();
         this.level = param2;
         this.achievementKey = param1;
      }
      
      public static function get instance() : RewardAchievementModule
      {
         return _instance || (_instance = new RewardAchievementModule());
      }
      
      override public function getLibcomModuleId() : int
      {
         return -6739;
      }
      
      override public function getLibcomModuleEstimatedLength() : int
      {
         return 6;
      }
      
      override public function read(param1:IDataInput) : void
      {
         param1.readShort();
         super.read(param1);
         this.level = param1.readInt();
         this.level = this.level >>> 6 | this.level << 26;
         this.achievementKey = param1.readUTF();
      }
      
      override public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-6739);
         super.write(param1);
         param1.writeInt(this.level << 6 | this.level >>> 26);
         param1.writeUTF(this.achievementKey);
      }
   }
}

