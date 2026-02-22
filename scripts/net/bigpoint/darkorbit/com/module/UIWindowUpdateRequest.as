package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class UIWindowUpdateRequest implements IModule
   {
      private static var _instance:UIWindowUpdateRequest;
      
      public var itemId:String = "";
      
      public var height:int = 0;
      
      public var width:int = 0;
      
      public var posX:int = 0;
      
      public var maximized:Boolean = false;
      
      public var posY:int = 0;
      
      public function UIWindowUpdateRequest(param1:String = "", param2:int = 0, param3:int = 0, param4:int = 0, param5:int = 0, param6:Boolean = false)
      {
         super();
         this.itemId = param1;
         this.height = param5;
         this.width = param4;
         this.posX = param2;
         this.maximized = param6;
         this.posY = param3;
      }
      
      public static function get instance() : UIWindowUpdateRequest
      {
         return _instance || (_instance = new UIWindowUpdateRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return 9246;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 19;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.itemId = param1.readUTF();
         this.height = param1.readInt();
         this.height = this.height >>> 2 | this.height << 30;
         this.width = param1.readInt();
         this.width = this.width >>> 10 | this.width << 22;
         this.posX = param1.readInt();
         this.posX = this.posX >>> 1 | this.posX << 31;
         this.maximized = param1.readBoolean();
         this.posY = param1.readInt();
         this.posY = this.posY >>> 9 | this.posY << 23;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(9246);
         param1.writeUTF(this.itemId);
         param1.writeInt(this.height << 2 | this.height >>> 30);
         param1.writeInt(this.width << 10 | this.width >>> 22);
         param1.writeInt(this.posX << 1 | this.posX >>> 31);
         param1.writeBoolean(this.maximized);
         param1.writeInt(this.posY << 9 | this.posY >>> 23);
      }
   }
}

