export class CampaignElement {
    id?: number;
    name?: string;
    message?: string;
    fileUrl?: string;
    sent?: number;
    read?: number;
    response: number;
    status: number;
    contactListIds: number[];
}
