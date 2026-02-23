package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   
   public class AddFireworkBoxCommand extends AddMapEntityCommand
   {
      private static var _instance:AddFireworkBoxCommand;
      
      public var colourCode:int = 0;
      
      public var eventModifier:String = "";
      
      public var typeId:int = 0;
      
      public function AddFireworkBoxCommand(param1:String = "", param2:int = 0, param3:int = 0, param4:int = 0, param5:int = 0, param6:String = "")
      {
         super(param1,param2,param3);
         this.colourCode = param5;
         this.eventModifier = param6;
         this.typeId = param4;
      }
      
      public static function get instance() : AddFireworkBoxCommand
      {
         return _instance || (_instance = new AddFireworkBoxCommand());
      }
      
      override public function getLibcomModuleId() : int
      {
         return 5613;
      }
      
      override public function getLibcomModuleEstimatedLength() : int
      {
         return 10;
      }
      
      override public function read(param1:IDataInput) : void
      {
         param1.readShort();
         super.read(param1);
         this.colourCode = param1.readInt();
         this.colourCode = this.colourCode << 7 | this.colourCode >>> 25;
         this.eventModifier = param1.readUTF();
         this.typeId = param1.readInt();
         this.typeId = this.typeId << 16 | this.typeId >>> 16;
      }
      
      override public function write(param1:IDataOutput) : void
      {
         param1.writeShort(5613);
         super.write(param1);
         param1.writeInt(this.colourCode >>> 7 | this.colourCode << 25);
         param1.writeUTF(this.eventModifier);
         param1.writeInt(this.typeId >>> 16 | this.typeId << 16);
      }
   }
}

