module Domain

open System

type AuthType = | Anonymous

type UrlId = Guid
type Url = string
type VisitCounter = uint

type ShortedUrl =
    { Id: UrlId
      SourceUrl: Url
      ShortedUrl: Url
      VisitCounter: VisitCounter
      UniqueCounter: VisitCounter
      CreatedAt: DateTime }

type UserId = Guid

type User =
    { Id: UserId
      AuthType: AuthType
      Username: string option
      Urls: ShortedUrl list }

type Errors =
    | LinkExist

let CreateUser authType username =
    { Id = Guid.NewGuid()
      AuthType = authType
      Username = username
      Urls = [] }
    
// Add rec counter check
let rec shortUrl url shortedUrls (lengthOfUrl: uint) =
    let alphabet = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789"

    let generateLink () = Random.Shared.GetItems(alphabet.ToCharArray(), int lengthOfUrl) |> string
    
    let uniqueCheck url =
        shortedUrls
        |> List.tryFind (fun elm -> elm = url)
        |> function
            | Some x -> Error LinkExist
            | None -> Ok url
            
    let url = match url with
                | Some link -> link
                | None -> generateLink()
                
    match uniqueCheck url with
    | Ok url -> url
    | Error _ -> shortUrl (Some(url)) shortedUrls lengthOfUrl
    
let CreateShortedUrl sourceUrl shortedUrl =
    failwith "todo"
