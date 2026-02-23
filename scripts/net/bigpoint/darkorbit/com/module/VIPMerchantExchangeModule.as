package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class VIPMerchantExchangeModule implements IModule
   {
      private static var _instance:VIPMerchantExchangeModule;
      
      public var isSpecial:Boolean = false;
      
      public var priority:int = 0;
      
      public var claimLimit:int = 0;
      
      public var claimCount:int = 0;
      
      public var costs:Vector.<VIPMerchantExchangeItemModule>;
      
      public var reward:VIPMerchantExchangeItemModule;
      
      public function VIPMerchantExchangeModule(param1:VIPMerchantExchangeItemModule = null, param2:Vector.<VIPMerchantExchangeItemModule> = null, param3:int = 0, param4:Boolean = false, param5:int = 0, param6:int = 0)
      {
         super();
         this.isSpecial = param4;
         this.priority = param3;
         this.claimLimit = param6;
         this.claimCount = param5;
         if(param2 == null)
         {
            this.costs = new Vector.<VIPMerchantExchangeItemModule>();
         }
         else
         {
            this.costs = param2;
         }
         if(param1 == null)
         {
            this.reward = new VIPMerchantExchangeItemModule();
         }
         else
         {
            this.reward = param1;
         }
      }
      
      public static function get instance() : VIPMerchantExchangeModule
      {
         return _instance || (_instance = new VIPMerchantExchangeModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return -28558;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 17;
      }
      
      public function read(param1:IDataInput) : void
      {
         var _loc4_:VIPMerchantExchangeItemModule = null;
         this.isSpecial = param1.readBoolean();
         this.priority = param1.readInt();
         this.priority = this.priority << 9 | this.priority >>> 23;
         this.claimLimit = param1.readInt();
         this.claimLimit = this.claimLimit >>> 1 | this.claimLimit << 31;
         this.claimCount = param1.readInt();
         this.claimCount = this.claimCount >>> 12 | this.claimCount << 20;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         while(this.costs.length > 0)
         {
            this.costs.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            _loc4_ = VIPMerchantExchangeItemModule(ModuleFactory.get().createInstance(param1.readShort()));
            _loc4_.read(param1);
            this.costs.push(_loc4_);
            _loc2_++;
         }
         this.reward = VIPMerchantExchangeItemModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.reward)
         {
            this.reward.read(param1);
         }
      }
      
      public function write(param1:IDataOutput) : void
      {
         var _loc2_:VIPMerchantExchangeItemModule = null;
         param1.writeShort(-28558);
         param1.writeBoolean(this.isSpecial);
         param1.writeInt(this.priority >>> 9 | this.priority << 23);
         param1.writeInt(this.claimLimit << 1 | this.claimLimit >>> 31);
         param1.writeInt(this.claimCount << 12 | this.claimCount >>> 20);
         param1.writeByte(this.costs.length);
         for each(_loc2_ in this.costs)
         {
            _loc2_.write(param1);
         }
         if(null != this.reward)
         {
            this.reward.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
      }
   }
}

