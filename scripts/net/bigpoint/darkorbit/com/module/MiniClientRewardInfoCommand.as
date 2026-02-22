package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class MiniClientRewardInfoCommand implements IModule
   {
      private static var _instance:MiniClientRewardInfoCommand;
      
      public var isClaimable:Boolean = false;
      
      public var daysClaimed:int = 0;
      
      public var timeToEndEvent:Number = 0;
      
      public var rewardList:Vector.<MiniClientItemRewardModule>;
      
      public function MiniClientRewardInfoCommand(param1:Vector.<MiniClientItemRewardModule> = null, param2:int = 0, param3:Number = 0, param4:Boolean = false)
      {
         super();
         this.isClaimable = param4;
         this.daysClaimed = param2;
         this.timeToEndEvent = param3;
         if(param1 == null)
         {
            this.rewardList = new Vector.<MiniClientItemRewardModule>();
         }
         else
         {
            this.rewardList = param1;
         }
      }
      
      public static function get instance() : MiniClientRewardInfoCommand
      {
         return _instance || (_instance = new MiniClientRewardInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 24234;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 15;
      }
      
      public function read(param1:IDataInput) : void
      {
         var _loc4_:MiniClientItemRewardModule = null;
         this.isClaimable = param1.readBoolean();
         this.daysClaimed = param1.readShort();
         this.daysClaimed = 65535 & ((65535 & this.daysClaimed) >>> 13 | (65535 & this.daysClaimed) << 3);
         this.daysClaimed = this.daysClaimed > 32767 ? int(this.daysClaimed - 65536) : this.daysClaimed;
         this.timeToEndEvent = param1.readDouble();
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
            _loc4_ = MiniClientItemRewardModule(ModuleFactory.get().createInstance(param1.readShort()));
            _loc4_.read(param1);
            this.rewardList.push(_loc4_);
            _loc2_++;
         }
      }
      
      public function write(param1:IDataOutput) : void
      {
         var _loc2_:MiniClientItemRewardModule = null;
         param1.writeShort(24234);
         param1.writeBoolean(this.isClaimable);
         param1.writeShort(65535 & ((65535 & this.daysClaimed) << 13 | (65535 & this.daysClaimed) >>> 3));
         param1.writeDouble(this.timeToEndEvent);
         param1.writeByte(this.rewardList.length);
         for each(_loc2_ in this.rewardList)
         {
            _loc2_.write(param1);
         }
      }
   }
}

