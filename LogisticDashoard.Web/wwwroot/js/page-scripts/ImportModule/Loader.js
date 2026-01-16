import { VesselStatus } from "./VesselStatus.js";
import { PortUtilizationManilaNorth } from "./PortUtilizationManilaNorth.js";
import { PortUtilizationBatangas } from "./PortUtilizationBatangas.js";
import { BerthingStatusManila } from "./BerthingStatusManilaNorth.js";
import { BerthingStatusBatangas } from "./BerthingStatusBatangas.js";
import { CriteriaControls } from "./CriteriaControls.js";
import { ImportDelivery } from "./ImportDelivery.js";

$(async function () {
    
    await VesselStatus();
    await ImportDelivery();
    await PortUtilizationManilaNorth();
    await PortUtilizationBatangas();
    await BerthingStatusManila();
    await BerthingStatusBatangas();
    

    await CriteriaControls();
});