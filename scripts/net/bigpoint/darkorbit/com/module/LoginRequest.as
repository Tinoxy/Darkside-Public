package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class LoginRequest implements IModule
   {
      private static var _instance:LoginRequest;
      
      public var userID:int = 0;
      
      public var factionID:int = 0;
      
      public var sessionID:String = "";
      
      public var version:String = "";
      
      public var instanceId:int = 0;
      
      public var isMiniClient:Boolean = false;
      
      public function LoginRequest(param1:int = 0, param2:int = 0, param3:String = "", param4:String = "", param5:int = 0, param6:Boolean = false)
      {
         super();
         this.userID = param1;
         this.factionID = param2;
         this.sessionID = param3;
         this.version = param4;
         this.instanceId = param5;
         this.isMiniClient = param6;
      }
      
      public static function get instance() : LoginRequest
      {
         return _instance || (_instance = new LoginRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return 7;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 15;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.userID = param1.readInt();
         this.userID = this.userID >>> 3 | this.userID << 29;
         this.factionID = param1.readShort();
         this.factionID = 65535 & ((65535 & this.factionID) << 9 | (65535 & this.factionID) >>> 7);
         this.factionID = this.factionID > 32767 ? int(this.factionID - 65536) : this.factionID;
         this.sessionID = param1.readUTF();
         this.version = param1.readUTF();
         this.instanceId = param1.readInt();
         this.instanceId = this.instanceId >>> 12 | this.instanceId << 20;
         this.isMiniClient = param1.readBoolean();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(7);
         param1.writeInt(this.userID << 3 | this.userID >>> 29);
         param1.writeShort(65535 & ((65535 & this.factionID) >>> 9 | (65535 & this.factionID) << 7));
         param1.writeUTF(this.sessionID);
         param1.writeUTF(this.version);
         param1.writeInt(this.instanceId << 12 | this.instanceId >>> 20);
         param1.writeBoolean(this.isMiniClient);
      }
   }
}

