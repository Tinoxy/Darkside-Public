package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class BattlePassKeyExchangeLootModule implements IModule
   {
      private static var _instance:BattlePassKeyExchangeLootModule;
      
      public var amount:int = 0;
      
      public var lootId:String = "";
      
      public function BattlePassKeyExchangeLootModule(param1:String = "", param2:int = 0)
      {
         super();
         this.amount = param2;
         this.lootId = param1;
      }
      
      public static function get instance() : BattlePassKeyExchangeLootModule
      {
         return _instance || (_instance = new BattlePassKeyExchangeLootModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return 9797;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 6;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.amount = param1.readInt();
         this.amount = this.amount << 9 | this.amount >>> 23;
         this.lootId = param1.readUTF();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(9797);
         param1.writeInt(this.amount >>> 9 | this.amount << 23);
         param1.writeUTF(this.lootId);
      }
   }
}

