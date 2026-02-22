package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class SlotBarConfigSetRequest implements IModule
   {
      private static var _instance:SlotBarConfigSetRequest;
      
      public var menuItemId:String = "";
      
      public var slotBarIdTo:String = "";
      
      public var slotBarIdFrom:String = "";
      
      public var slotIdTo:int = 0;
      
      public var slotIdFrom:int = 0;
      
      public function SlotBarConfigSetRequest(param1:String = "", param2:int = 0, param3:String = "", param4:int = 0, param5:String = "")
      {
         super();
         this.menuItemId = param5;
         this.slotBarIdTo = param3;
         this.slotBarIdFrom = param1;
         this.slotIdTo = param4;
         this.slotIdFrom = param2;
      }
      
      public static function get instance() : SlotBarConfigSetRequest
      {
         return _instance || (_instance = new SlotBarConfigSetRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return 31832;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 14;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.menuItemId = param1.readUTF();
         this.slotBarIdTo = param1.readUTF();
         this.slotBarIdFrom = param1.readUTF();
         this.slotIdTo = param1.readInt();
         this.slotIdTo = this.slotIdTo >>> 10 | this.slotIdTo << 22;
         this.slotIdFrom = param1.readInt();
         this.slotIdFrom = this.slotIdFrom >>> 11 | this.slotIdFrom << 21;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(31832);
         param1.writeUTF(this.menuItemId);
         param1.writeUTF(this.slotBarIdTo);
         param1.writeUTF(this.slotBarIdFrom);
         param1.writeInt(this.slotIdTo << 10 | this.slotIdTo >>> 22);
         param1.writeInt(this.slotIdFrom << 11 | this.slotIdFrom >>> 21);
      }
   }
}

