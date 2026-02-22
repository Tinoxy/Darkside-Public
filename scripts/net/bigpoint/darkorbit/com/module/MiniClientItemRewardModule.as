package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class MiniClientItemRewardModule implements IModule
   {
      private static var _instance:MiniClientItemRewardModule;
      
      public var amount:int = 0;
      
      public var lootId:String = "";
      
      public function MiniClientItemRewardModule(param1:String = "", param2:int = 0)
      {
         super();
         this.amount = param2;
         this.lootId = param1;
      }
      
      public static function get instance() : MiniClientItemRewardModule
      {
         return _instance || (_instance = new MiniClientItemRewardModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return 13464;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 6;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.amount = param1.readInt();
         this.amount = this.amount << 16 | this.amount >>> 16;
         this.lootId = param1.readUTF();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(13464);
         param1.writeInt(this.amount >>> 16 | this.amount << 16);
         param1.writeUTF(this.lootId);
      }
   }
}

