import { AxiosInstance } from "axios"

export type ClientWithAxiosInstance<TClient> = {
    instance: AxiosInstance;
    client: TClient;
}