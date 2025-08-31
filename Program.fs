open System

module Clipboard = 
    module Entry =
        type t = string

    module History =
				type t
        let empty : Entry.t list = []
        let get_current_clipboard_content =
            let is_text = Windows.Forms.Clipboard.ContainsText() in
            match is_text with
            | true -> Some (Windows.Forms.Clipboard.GetText())
            | false -> None

        let add history = 
            let entry_content = get_current_clipboard_content
            in
            match entry_content with
            | Some text -> List.append history [Entry.t text]
            | None -> history
        
        let latest_entry history = List.last history
        let new_entry_detected history =
            let current_clipboard_content = get_current_clipboard_content
            in
            match current_clipboard_content with
            // why is fsharp like this lol. List.append can take multiple args w/o parens,
            // but String.Equals refuses to compile unless in parens
            // None may mean there is data that is not text on the clipboard
            | None -> false
            // May want to make this not detect a new entry if the data on clipboard is in the last n (say, 10)
            // history entries
            | Some c when String.Equals(latest_entry history, c) -> false
            | _ -> true

// TODO get last n entries to show

// TODO add a State monad for handling state..or accept defeat and decide to mutate history

// Poll for new data

// someday: support adding a permanent entry and dynamic entries (like formatting the date into an entry)

[<EntryPoint; STAThread>]
let main argv =
    let clipboard_history= Clipboard.History.add Clipboard.History.empty
    // TODO run this in a loop -- listen until quit and print new entries with ordered history as they happen
    printfn "This is what was last copied to the clipboard: %s" (Clipboard.History.latest_entry clipboard_history)
    printfn "This is the clipboard history"
    // TODO once i have a 'poc' working under the hood as i want it to, add a gui
    0 // TODO return an appropriate integer exit code
