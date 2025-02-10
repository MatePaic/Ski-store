export type ProductParams = {
    sort: string,
    search?: string,
    types: string[],
    brands: string[],
    pageNumber: number,
    pageSize: number
}