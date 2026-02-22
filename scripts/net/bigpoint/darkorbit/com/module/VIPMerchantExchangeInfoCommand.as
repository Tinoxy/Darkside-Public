package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class VIPMerchantExchangeInfoCommand implements IModule
   {
      private static var _instance:VIPMerchantExchangeInfoCommand;
      
      public var packagesBought:int = 0;
      
      public var timeToEventEnd:Number = 0;
      
      public var exchangeList:Vector.<VIPMerchantExchangeModule>;
      
      public var packageList:Vector.<String>;
      
      public function VIPMerchantExchangeInfoCommand(param1:Vector.<VIPMerchantExchangeModule> = null, param2:Vector.<String> = null, param3:int = 0, param4:Number = 0)
      {
         super();
         this.packagesBought = param3;
         this.timeToEventEnd = param4;
         if(param1 == null)
         {
            this.exchangeList = new Vector.<VIPMerchantExchangeModule>();
         }
         else
         {
            this.exchangeList = param1;
         }
         if(param2 == null)
         {
            this.packageList = new Vector.<String>();
         }
         else
         {
            this.packageList = param2;
         }
      }
      
      public static function get instance() : VIPMerchantExchangeInfoCommand
      {
         return _instance || (_instance = new VIPMerchantExchangeInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -25212;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 18;
      }
      
      public function read(param1:IDataInput) : void
      {
         var _loc4_:VIPMerchantExchangeModule = null;
         this.packagesBought = param1.readShort();
         this.packagesBought = 65535 & ((65535 & this.packagesBought) << 3 | (65535 & this.packagesBought) >>> 13);
         this.packagesBought = this.packagesBought > 32767 ? int(this.packagesBought - 65536) : this.packagesBought;
         this.timeToEventEnd = param1.readDouble();
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         while(this.exchangeList.length > 0)
         {
            this.exchangeList.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            _loc4_ = VIPMerchantExchangeModule(ModuleFactory.get().createInstance(param1.readShort()));
            _loc4_.read(param1);
            this.exchangeList.push(_loc4_);
            _loc2_++;
         }
         while(this.packageList.length > 0)
         {
            this.packageList.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            this.packageList.push(param1.readUTF());
            _loc2_++;
         }
      }
      
      public function write(param1:IDataOutput) : void
      {
         var _loc2_:VIPMerchantExchangeModule = null;
         var _loc3_:* = null;
         param1.writeShort(-25212);
         param1.writeShort(65535 & ((65535 & this.packagesBought) >>> 3 | (65535 & this.packagesBought) << 13));
         param1.writeDouble(this.timeToEventEnd);
         param1.writeByte(this.exchangeList.length);
         for each(_loc2_ in this.exchangeList)
         {
            _loc2_.write(param1);
         }
         param1.writeByte(this.packageList.length);
         for each(_loc3_ in this.packageList)
         {
            param1.writeUTF(_loc3_);
         }
      }
   }
}

