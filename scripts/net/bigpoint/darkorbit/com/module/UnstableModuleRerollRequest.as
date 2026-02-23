package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class UnstableModuleRerollRequest implements IModule
   {
      private static var _instance:UnstableModuleRerollRequest;
      
      public var itemId:int = 0;
      
      public var rerollCost:int = 0;
      
      public var rerollCount:int = 0;
      
      public function UnstableModuleRerollRequest(param1:int = 0, param2:int = 0, param3:int = 0)
      {
         super();
         this.itemId = param1;
         this.rerollCost = param2;
         this.rerollCount = param3;
      }
      
      public static function get instance() : UnstableModuleRerollRequest
      {
         return _instance || (_instance = new UnstableModuleRerollRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return -1785;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 12;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.itemId = param1.readInt();
         this.itemId = this.itemId << 9 | this.itemId >>> 23;
         this.rerollCost = param1.readInt();
         this.rerollCost = this.rerollCost >>> 15 | this.rerollCost << 17;
         this.rerollCount = param1.readInt();
         this.rerollCount = this.rerollCount << 10 | this.rerollCount >>> 22;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-1785);
         param1.writeInt(this.itemId >>> 9 | this.itemId << 23);
         param1.writeInt(this.rerollCost << 15 | this.rerollCost >>> 17);
         param1.writeInt(this.rerollCount >>> 10 | this.rerollCount << 22);
      }
   }
}

