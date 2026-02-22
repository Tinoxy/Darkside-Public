package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class ReturneeLoginInitialRewardInfoCommand implements IModule
   {
      private static var _instance:ReturneeLoginInitialRewardInfoCommand;
      
      public var isClaimable:Boolean = false;
      
      public var rewardList:Vector.<ReturneeLoginItemRewardModule>;
      
      public function ReturneeLoginInitialRewardInfoCommand(param1:Vector.<ReturneeLoginItemRewardModule> = null, param2:Boolean = false)
      {
         super();
         this.isClaimable = param2;
         if(param1 == null)
         {
            this.rewardList = new Vector.<ReturneeLoginItemRewardModule>();
         }
         else
         {
            this.rewardList = param1;
         }
      }
      
      public static function get instance() : ReturneeLoginInitialRewardInfoCommand
      {
         return _instance || (_instance = new ReturneeLoginInitialRewardInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 22731;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 5;
      }
      
      public function read(param1:IDataInput) : void
      {
         var _loc4_:ReturneeLoginItemRewardModule = null;
         this.isClaimable = param1.readBoolean();
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         while(this.rewardList.length > 0)
         {
            this.rewardList.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            _loc4_ = ReturneeLoginItemRewardModule(ModuleFactory.get().createInstance(param1.readShort()));
            _loc4_.read(param1);
            this.rewardList.push(_loc4_);
            _loc2_++;
         }
      }
      
      public function write(param1:IDataOutput) : void
      {
         var _loc2_:ReturneeLoginItemRewardModule = null;
         param1.writeShort(22731);
         param1.writeBoolean(this.isClaimable);
         param1.writeByte(this.rewardList.length);
         for each(_loc2_ in this.rewardList)
         {
            _loc2_.write(param1);
         }
      }
   }
}

