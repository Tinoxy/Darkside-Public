package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class SeasonPassRewardClaimRequest implements IModule
   {
      private static var _instance:SeasonPassRewardClaimRequest;
      
      public var extGoldPass:Boolean = false;
      
      public var goldPass:Boolean = false;
      
      public var level:int = 0;
      
      public function SeasonPassRewardClaimRequest(param1:int = 0, param2:Boolean = false, param3:Boolean = false)
      {
         super();
         this.extGoldPass = param3;
         this.goldPass = param2;
         this.level = param1;
      }
      
      public static function get instance() : SeasonPassRewardClaimRequest
      {
         return _instance || (_instance = new SeasonPassRewardClaimRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return -15195;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 6;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.extGoldPass = param1.readBoolean();
         this.goldPass = param1.readBoolean();
         this.level = param1.readInt();
         this.level = this.level >>> 10 | this.level << 22;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-15195);
         param1.writeBoolean(this.extGoldPass);
         param1.writeBoolean(this.goldPass);
         param1.writeInt(this.level << 10 | this.level >>> 22);
      }
   }
}

