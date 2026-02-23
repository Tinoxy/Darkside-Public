package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class VIPMerchantExchangeItemModule implements IModule
   {
      private static var _instance:VIPMerchantExchangeItemModule;
      
      public var amountOwned:Number = 0;
      
      public var lootId:String = "";
      
      public var amount:int = 0;
      
      public function VIPMerchantExchangeItemModule(param1:String = "", param2:int = 0, param3:Number = 0)
      {
         super();
         this.amountOwned = param3;
         this.lootId = param1;
         this.amount = param2;
      }
      
      public static function get instance() : VIPMerchantExchangeItemModule
      {
         return _instance || (_instance = new VIPMerchantExchangeItemModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return 30463;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 14;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.amountOwned = param1.readDouble();
         this.lootId = param1.readUTF();
         this.amount = param1.readInt();
         this.amount = this.amount << 10 | this.amount >>> 22;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(30463);
         param1.writeDouble(this.amountOwned);
         param1.writeUTF(this.lootId);
         param1.writeInt(this.amount >>> 10 | this.amount << 22);
      }
   }
}

