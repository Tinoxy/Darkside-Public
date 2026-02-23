package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class InvitationInitializationCommand implements IModule
   {
      private static var _instance:InvitationInitializationCommand;
      
      public var inviterName:String = "";
      
      public var inviterId:int = 0;
      
      public var candidateShip:ShipIconModule;
      
      public var candidateName:String = "";
      
      public var candidateId:int = 0;
      
      public var inviterShip:ShipIconModule;
      
      public function InvitationInitializationCommand(param1:int = 0, param2:String = "", param3:ShipIconModule = null, param4:int = 0, param5:String = "", param6:ShipIconModule = null)
      {
         super();
         this.inviterName = param2;
         this.inviterId = param1;
         if(param6 == null)
         {
            this.candidateShip = new ShipIconModule();
         }
         else
         {
            this.candidateShip = param6;
         }
         this.candidateName = param5;
         this.candidateId = param4;
         if(param3 == null)
         {
            this.inviterShip = new ShipIconModule();
         }
         else
         {
            this.inviterShip = param3;
         }
      }
      
      public static function get instance() : InvitationInitializationCommand
      {
         return _instance || (_instance = new InvitationInitializationCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -14564;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 12;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.inviterName = param1.readUTF();
         this.inviterId = param1.readInt();
         this.inviterId = this.inviterId << 5 | this.inviterId >>> 27;
         this.candidateShip = ShipIconModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.candidateShip)
         {
            this.candidateShip.read(param1);
         }
         this.candidateName = param1.readUTF();
         this.candidateId = param1.readInt();
         this.candidateId = this.candidateId >>> 6 | this.candidateId << 26;
         this.inviterShip = ShipIconModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.inviterShip)
         {
            this.inviterShip.read(param1);
         }
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-14564);
         param1.writeUTF(this.inviterName);
         param1.writeInt(this.inviterId >>> 5 | this.inviterId << 27);
         if(null != this.candidateShip)
         {
            this.candidateShip.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         param1.writeUTF(this.candidateName);
         param1.writeInt(this.candidateId << 6 | this.candidateId >>> 26);
         if(null != this.inviterShip)
         {
            this.inviterShip.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
      }
   }
}

