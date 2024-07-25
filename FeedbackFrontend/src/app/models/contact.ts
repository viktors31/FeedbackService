export type TContact = {
    id: number,
    mail: string,
    phone: string,
    name: string,
}

export interface IContact {
    id: number;
    mail: string;
    phone: string;
    name: string;
}

export class Contact {
    public id: number;
    public mail: string;
    public phone: string;
    public name: string;

    constructor(id: number, mail: string, phone: string, name: string,) {
        this.id = id;
        this.mail = mail;
        this.phone = phone;
        this.name = name;
    }
}